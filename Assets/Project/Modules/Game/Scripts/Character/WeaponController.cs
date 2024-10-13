using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private LayerMask _weaponLayerMask;
        [SerializeField] private WeaponConfig _weaponConfig;

        [Header("Components")]
        [SerializeField] private Transform _shootPoint;

        public float Range => this._weaponConfig.Range;
        public bool CanShoot => this._lastTimeShot > this._weaponConfig.ShootInterval;

        private float _lastTimeShot;
        private int _weaponLayerIndex;

        private void OnValidate()
        {
            if (this._shootPoint == null)
                this._shootPoint = this.transform;

            WeaponConfigDatabase database = Resources.Load<WeaponConfigDatabase>(ProjectPaths.WEAPON_CONFIG_DATABASE);
            this._weaponConfig = database.Config[(int)this._weaponType];
        }

        private void Awake()
        {
            this._weaponLayerIndex = Mathf.RoundToInt(Mathf.Log(this._weaponLayerMask.value, 2));
        }

        private void Update()
        {
            this._lastTimeShot += Time.deltaTime;
        }

        public void Shoot()
        {
            Bullet bullet = SpawnablePool.SpawnProjectile<Bullet>(SpawnTypeProjectile.ENERGY_MISSILE);
            bullet.gameObject.layer = this._weaponLayerIndex;
            bullet.SetWeaponConfig(this._weaponConfig);
            bullet.Shoot(this._shootPoint);

            this._lastTimeShot = 0f;
        }
    }
}