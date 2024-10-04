using Game.Database;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Game
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private LayerMask _weaponLayerMask;

        [Header("Components")]
        [SerializeField] private Transform _shootPoint;
        [SerializeField, ReadOnly] private WeaponDatabase _weaponDatabase;

        public float Range => this._weaponConfig.Range;
        public bool CanShoot => this._lastTimeShot > this._weaponConfig.ShootInterval;

        private float _lastTimeShot;
        private int _weaponLayerIndex;
        private WeaponConfig _weaponConfig;

        private void OnValidate()
        {
            if (this._shootPoint == null)
                this._shootPoint = this.transform;

            if (this._weaponDatabase == null)
                this._weaponDatabase = Resources.Load<WeaponDatabase>(ProjectPaths.WEAPON_DATABASE);
        }

        private void Awake()
        {
            this._weaponConfig = this._weaponDatabase.Weapons[(int)this._weaponType];
            this._weaponLayerIndex = Mathf.RoundToInt(Mathf.Log(this._weaponLayerMask.value, 2));
        }

        private void Update()
        {
            this._lastTimeShot += Time.deltaTime;
        }

        public void Shoot()
        {
            bool isGamepadConnected = Gamepad.current != null;
            if (isGamepadConnected)
            {
                Vector2 aimDirection = Gamepad.current.leftStick.ReadValue();
                this._shootPoint.forward = aimDirection;
                this.Shoot(aimDirection);
            }
            else
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                this.ShootAtTarget(new Vector3(mousePosition.x, 0, mousePosition.y));
            }
        }

        public void ShootAtTarget(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - this._shootPoint.position).normalized;
            this.Shoot(directionToTarget);
        }

        private void Shoot(Vector3 direction)
        {
            Bullet bullet = SpawnablePool.SpawnProjectile<Bullet>(SpawnTypeProjectile.ENERGY_MISSILE);
            bullet.gameObject.layer = this._weaponLayerIndex;
            bullet.SetWeaponConfig(this._weaponConfig);
            bullet.Shoot(this._shootPoint, Quaternion.LookRotation(direction));

            this._lastTimeShot = 0f;
        }
    }
}