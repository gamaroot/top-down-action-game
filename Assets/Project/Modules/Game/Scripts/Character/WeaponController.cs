using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private Tags _weaponTag;
        [SerializeField] private LayerMask _weaponLayerMask;

        [Header("Components")]
        [SerializeField] private Transform _shootPoint;

        public float Range => this._config.Range;
        public bool CanShoot => this._lastTimeShot > this._config.ShootInterval;

        private float _lastTimeShot;
        private int _weaponLayerIndex;
        private IWeaponConfig _config;

        private void OnValidate()
        {
            if (this._shootPoint == null)
                this._shootPoint = this.transform;
        }

        private void Awake()
        {
            this._weaponLayerIndex = Mathf.RoundToInt(Mathf.Log(this._weaponLayerMask.value, 2));
        }

        private void Update()
        {
            this._lastTimeShot += Time.deltaTime;
        }

        public void Init(IWeaponConfig[] config)
        {
            this._config = config[(int)this._weaponType];
        }

        public void Shoot()
        {
            Bullet bullet = SpawnablePool.SpawnBullet(this._weaponType);
            bullet.tag = this._weaponTag.ToString();
            bullet.gameObject.layer = this._weaponLayerIndex;
            bullet.Setup(this._config);
            bullet.Shoot(this._shootPoint);

            this._lastTimeShot = 0f;
        }
    }
}