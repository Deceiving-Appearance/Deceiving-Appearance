using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Build Settings")]
    [SerializeField] private GameObject cameraScanner;
    [SerializeField] private GameObject cameraScannerPlaceholderValid;
    [SerializeField] private GameObject cameraScannerPlaceholderInvalid;
    [SerializeField] private GameObject LidarGrenadePrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private float maxBuildDistance = 5f;

    [SerializeField] int lidarGrenadeAmount = 3;

    private InputAction _build;
    private InputAction _place;
    private InputAction _throw;
    private bool _buildMode = false;

    void Start()
    {
        _build = playerInput.actions["Build"];
        _place = playerInput.actions["Fire"];
        _throw = playerInput.actions["Throw"];

        _build.performed += ToggleBuildMode;
        _place.performed += PlaceCameraScanner;
    }

    void Update()
    {
        if (_buildMode)
        {
            UpdatePlaceholderPosition();
        }
        else
        {
            cameraScannerPlaceholderValid.SetActive(false);
            cameraScannerPlaceholderInvalid.SetActive(false);
        }
    }

    void ToggleBuildMode(InputAction.CallbackContext context)
    {
        _buildMode = !_buildMode;
        Debug.Log(_buildMode ? "Build mode on" : "Build mode off");
    }

    void UpdatePlaceholderPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxBuildDistance, placementLayerMask))
        {
            cameraScannerPlaceholderValid.transform.position = hit.point;

            float angleFromUp = Vector3.Angle(hit.normal, Vector3.up);
            float threshold = 10f;

            if (angleFromUp <= threshold)
            {
                Vector3 flatDirection = (transform.position - hit.point);
                flatDirection.y = 0;
                flatDirection = flatDirection.sqrMagnitude > 0.001f ? flatDirection.normalized : Vector3.forward;

                Quaternion yOnlyRotation = Quaternion.LookRotation(-flatDirection, Vector3.up);
                cameraScannerPlaceholderValid.transform.rotation = yOnlyRotation;
            }
            else
            {
                Vector3 awayFromPlayer = (hit.point - transform.position).normalized;
                Quaternion surfaceRotation = Quaternion.LookRotation(awayFromPlayer, hit.normal);
                cameraScannerPlaceholderValid.transform.rotation = surfaceRotation;
            }

            cameraScannerPlaceholderValid.SetActive(true);
            cameraScannerPlaceholderInvalid.SetActive(false);
        }
        else
        {
            // Fallback to placing in front of player
            Vector3 forwardFlat = playerCamera.transform.forward;
            forwardFlat.y = -5;
            forwardFlat = forwardFlat.normalized;

            Vector3 fallbackPosition = playerCamera.transform.position + forwardFlat * maxBuildDistance;
            Quaternion fallbackRotation = Quaternion.LookRotation(-forwardFlat, Vector3.up);

            cameraScannerPlaceholderInvalid.transform.position = fallbackPosition;
            cameraScannerPlaceholderInvalid.transform.rotation = fallbackRotation;

            cameraScannerPlaceholderValid.SetActive(false);
            cameraScannerPlaceholderInvalid.SetActive(true);
        }
    }

    void PlaceCameraScanner(InputAction.CallbackContext context)
    {
        if (!_buildMode) return;

        GameObject activePlaceholder = cameraScannerPlaceholderValid.activeSelf
            ? cameraScannerPlaceholderValid
            : cameraScannerPlaceholderInvalid.activeSelf
                ? cameraScannerPlaceholderInvalid
                : null;

        if (activePlaceholder != null)
        {
            Vector3 spawnPosition = activePlaceholder.transform.position;
            Quaternion spawnRotation = activePlaceholder.transform.rotation;

            Instantiate(cameraScanner, spawnPosition, spawnRotation);
            Debug.Log("Camera scanner built.");

            _buildMode = false;
            cameraScannerPlaceholderValid.SetActive(false);
            cameraScannerPlaceholderInvalid.SetActive(false);
        }
    }

    // void ThrowLidarGrenade()
    // {
    //     if (lidarGrenadeAmount > 0)
    //     {
    //         // TODO - Instance grenade and use force to throw.
    //         GameObject grenade = Instantiate(LidarGrenadePrefab, threwPoint.position, throwPoint.rotation);
    //         Rigidbody rb = grenade.GetComponent<Rigidbody>();
    //     }
    // }

    void OnDestroy()
    {
        _build.performed -= ToggleBuildMode;
        _place.performed -= PlaceCameraScanner;
    }
}