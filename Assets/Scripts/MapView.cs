using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LRS
{
    public class MapView : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCamera playerCameraController;

        [Header("Map Settings")]
        [SerializeField] private GameObject mapCamera;
        [SerializeField] private GameObject playerCameraObj;
        [SerializeField] private GameObject playerObj;
        [SerializeField] private bool _mapMode;
        [SerializeField] private GameObject mapLidarPointObj;
        [SerializeField] private Vector3 mapOffset;
        [SerializeField] private float mapMoveSpeed;

        private InputAction _map;
        private InputAction _move;
        private Vector3 _moveDirection;

        void Start()
        {
            _map = playerInput.actions["Map"];
            _move = playerInput.actions["Move"];

            _map.performed += ToggleMapMode;
        }

        void Update()
        {
            if (_mapMode)
            {
                GetMovementDirection();
            }
        }

        void ToggleMapMode(InputAction.CallbackContext context)
        {
            _mapMode = !_mapMode;
            Debug.Log(_mapMode ? "Map mode on" : "Map mode off");
            
            mapCamera.transform.position = playerObj.transform.position + mapOffset;
            mapCamera.transform.LookAt(playerObj.transform.position);
            
            SwitchMapView();
        }

        void SwitchMapView()
        {
            if (_mapMode)
            {
                mapCamera.SetActive(true);
                playerCameraObj.SetActive(false);
                playerMovement.pauseMovement = true;
                playerCameraController.pauseCameraMovement = true;
            }
            else
            {
                mapCamera.SetActive(false);
                playerCameraObj.SetActive(true);
                playerMovement.pauseMovement = false;
                playerCameraController.pauseCameraMovement = false;
            }
        }

        void MoveMap(InputAction.CallbackContext context)
        {
            if (_mapMode)
            {
                GetMovementDirection();
            }
        }

        private void GetMovementDirection()
        {
            float horizontalMovement = _move.ReadValue<Vector2>().x;
            float verticalMovement = _move.ReadValue<Vector2>().y;

            Vector3 forward = mapCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = mapCamera.transform.right;
            right.y = 0;
            right.Normalize();
            
            _moveDirection = (right * horizontalMovement + forward * verticalMovement).normalized;
            mapCamera.transform.position += _moveDirection * mapMoveSpeed * Time.deltaTime;
            Debug.Log(_moveDirection);
        }

        // private void OrbitCameraControl()
        // {
        //     float mouseX = _look.ReadValue<Vector2>().x;
        //     float mouseY = _look.ReadValue<Vector2>().y;
    
        //     _yRotation += GetMouseInput().x * _sensitivity * Time.deltaTime;
            
        // }
    }
}