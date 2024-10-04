using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(WeaponController))]
    public class PlayerShootController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private WeaponController _weapon;

        private InputController _inputs;

        private void OnValidate()
        {
            if (this._weapon == null)
                this._weapon = base.GetComponent<WeaponController>();
        }

        private void Awake()
        {
            this._inputs = new InputController();
            this._inputs.Player.Fire.performed += context => this.OnFirePressed(context.ReadValue<Vector2>());
        }

        private void OnFirePressed(Vector2 point)
        {
            if (this._weapon.CanShoot)
                this._weapon.ShootAtTarget(new Vector3(point.x, 0, point.y));
        }
    }
}