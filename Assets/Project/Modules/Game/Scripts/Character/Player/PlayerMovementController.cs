using Game.Database;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils;

namespace Game
{
    public class PlayerMovementController : MovementController
    {
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDashStart;
        [SerializeField] private UnityEvent _onDashEnd;

        private float MovementSpeed => this._stats.MovementSpeed;
        private float DashSpeed => this._stats.DashSpeed;
        private float DashDuration => this._stats.DashDuration;
        private float DashCooldown => this._stats.DashCooldown;
        private Vector3 NormalMovement => this.MovementSpeed * Time.fixedDeltaTime * new Vector3(this._move.x, 0, this._move.y);

        private Vector2 _move;
        private InputController _inputs;
        private DashHandler _dashHandler;
        private CharacterStats _stats;

        private void OnValidate()
        {
            this._rigidbody ??= base.GetComponent<Rigidbody>();
        }

        private void Awake()
        {
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

        private void FixedUpdate()
        {
            this._dashHandler.OnUpdate();

            this.Move(this._dashHandler.IsDashing ? this._dashHandler.DashMovement : this.NormalMovement);
        }

        public void Init(CharacterStats stats)
        {
            this._stats = stats;

            this._dashHandler = new(this.DashSpeed, this.DashDuration, this.DashCooldown)
            {
                OnDashStart = this._onDashStart,
                OnDashEnd = this._onDashEnd
            };
        }

        public override void Move(Vector3 point)
        {
            if (point == Vector3.zero)
                return;

            this._rigidbody.MovePosition(this._rigidbody.position + point);
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