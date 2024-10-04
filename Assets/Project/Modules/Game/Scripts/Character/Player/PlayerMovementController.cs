using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MovementController
    {
        [Header("Attributes")]
        [SerializeField] private float _speed = 10f;

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
            if (this._controller.enabled)
            {
                Vector3 movement = this._speed * Time.deltaTime * new Vector3(this._move.x, 0, this._move.y);
                this._controller.Move(movement);
            }
        }

        public override void Move(Vector3 point)
        {
            this._controller.Move(point);
        }
    }
}