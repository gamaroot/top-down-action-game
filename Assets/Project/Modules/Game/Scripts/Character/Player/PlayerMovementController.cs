using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MovementController
    {
        [Header("Attributes")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _dashSpeed = 30f;      // Speed multiplier during dash
        [SerializeField] private float _dashDuration = 0.2f;  // Duration of the dash
        [SerializeField] private float _dashCooldown = 1f;    // Cooldown time between dashes

        [Header("Components")]
        [SerializeField] private CharacterController _controller;

        private Vector3 NormalMovement => this._speed * Time.deltaTime * new Vector3(this._move.x, 0, this._move.y);

        private Vector2 _move;
        private InputController _inputs;
        private DashHandler _dashHandler;

        private void OnValidate()
        {
            if (this._controller == null)
                this._controller = base.GetComponent<CharacterController>();
        }

        private void Awake()
        {
            this._dashHandler = new(this._dashSpeed, this._dashDuration, this._dashCooldown);

            this._inputs = new InputController();
            this._inputs.Player.Move.performed += context => this._move = context.ReadValue<Vector2>();
            this._inputs.Player.Move.canceled += context => this._move = Vector2.zero;
            this._inputs.Player.Dash.performed += context => this.OnDashPressed();
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
            this._dashHandler.OnUpdate();

            this.Move(this._dashHandler.IsDashing ? this._dashHandler.DashMovement : this.NormalMovement);
        }

        public override void Move(Vector3 point)
        {
            if (point == Vector3.zero)
                return;

            this._controller.Move(point);
        }

        private void OnDashPressed()
        {
            if (!this._dashHandler.CanDash)
                return;

            bool isGamepad = Gamepad.current != null;
            Vector2 dashDirection = isGamepad ? Gamepad.current.leftStick.value : this._move;
            this._dashHandler.Dash(dashDirection);
        }
    }
}