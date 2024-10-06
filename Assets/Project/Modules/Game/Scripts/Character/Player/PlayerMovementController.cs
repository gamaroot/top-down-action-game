using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Utils;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MovementController
    {
        [Header("Attributes")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private LayerMask _groundLayerMask;

        [Header("Components")]
        [SerializeField] private CharacterController _controller;

        private Vector2 _move;
        private InputController _inputs;

        private void OnValidate()
        {
            if (this._controller == null)
                this._controller = base.GetComponent<CharacterController>();
        }

        private void Awake()
        {
            this._inputs = new InputController();
            this._inputs.Player.Move.performed += context => this._move = context.ReadValue<Vector2>();
            this._inputs.Player.Move.canceled += context => this._move = Vector2.zero;
        }

        private void OnEnable()
        {
            this._inputs.Enable();
        }

        private void OnDisable()
        {
            this._inputs.Disable();
        }

        private void Update()
        {
            Vector3 movement = this._speed * Time.deltaTime * new Vector3(this._move.x, 0, this._move.y);
            this.Move(movement);

            if (Gamepad.current != null)
                this.RotateToGamepadDirection();
            else
                this.RotateToMousePoint();
        }

        public override void Move(Vector3 point)
        {
            if (point == Vector3.zero)
                return;

            this._controller.Move(point);
        }

        private void RotateToMousePoint()
        {
            Vector2 mousePosition = Mouse.current.position.value;
            Ray ray = CameraHandler.Instance.MainCamera.ScreenPointToRay(mousePosition);

            // Check if the ray hits the ground
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this._groundLayerMask))
            {
                // Get the direction from the player to the hit point
                Vector3 direction = hit.point - this._controller.transform.position;

                this.RotateToDirection(new Vector3(direction.x, direction.z));
            }
        }

        private void RotateToGamepadDirection()
        {
            Vector2 direction = Gamepad.current.rightStick.value;
            this.RotateToDirection(direction);
        }

        private void RotateToDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            // Ensure the direction vector is normalized and ignore the y-axis
            var flatDirection = new Vector3(direction.x, 0, direction.y);
            this._controller.transform.rotation = Quaternion.LookRotation(flatDirection);
        }
    }
}