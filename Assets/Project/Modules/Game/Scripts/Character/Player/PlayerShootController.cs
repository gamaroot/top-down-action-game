using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerShootController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private WeaponController _weapon;

        private bool _isFirePressed;
        private InputController _inputs;

        private void OnValidate()
        {
            if (this._weapon == null)
                this._weapon = base.GetComponent<WeaponController>();
        }

        private void Awake()
        {
            this._inputs = new InputController();
            this._inputs.Player.Fire.performed += _ => this._isFirePressed = true;
            this._inputs.Player.Fire.canceled += _ => this._isFirePressed = false;
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
            if (this._isFirePressed)
                this.OnFirePressed();
        }

        private void OnFirePressed()
        {
            // Check if the gamepad is not connected and the pointer is over a UI element
            if (Gamepad.current == null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (this._weapon.CanShoot)
                this._weapon.Shoot();
        }
    }
}