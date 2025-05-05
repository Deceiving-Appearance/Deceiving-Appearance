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
        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] Transform mapPivot;

        private InputAction _map;
        private InputAction _move;
        private InputAction _mapScroll;
        private InputAction _mapRotate;
        private Vector3 _moveDirection;
        private Vector2 _rotateDirection;

        void Start()
        {
            _map = playerInput.actions["Map"];
            _move = playerInput.actions["Move"];
            _mapScroll = playerInput.actions["Scroll"];
            _mapRotate = playerInput.actions["Rotate"];

            _map.performed += ToggleMapMode;
        }

        void Update()
        {
            if (_mapMode)
            {
                GetMovementDirection();
                MapZoom();
                GetRotateMap();
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
        }

        public void GetRotateMap()
        {
            float rotateValue = _mapRotate.ReadValue<float>();

            mapCamera.transform.RotateAround(mapPivot.transform.position, Vector3.up, rotateValue * rotationSpeed * Time.deltaTime);
        }

        private void MapZoom()
        {
            float scrollValue = _mapScroll.ReadValue<float>();

            if (scrollValue == 0)
            {
                return;
            }

            mapCamera.transform.position += mapCamera.transform.forward * scrollValue * scrollSpeed * Time.deltaTime;
        }
    }
}