using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(ParticleSystem), typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] private ParticleSystem _particleSystem;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;

        [Header("Others")]
        [SerializeField, ReadOnly] private Color _originalColor;
        [SerializeField, ReadOnly] private Color _pinkColor;

        public bool IsPinky { get; private set; }
        private WeaponConfig _weaponConfig;

        private void OnValidate()
        {
            if (this._particleSystem == null)
                this._particleSystem = this.GetComponent<ParticleSystem>();

            if (this._rigidbody == null)
                this._rigidbody = base.GetComponent<Rigidbody>();

            this._originalColor = this._particleSystem.main.startColor.color;
            this._pinkColor = Color.magenta;
        }

        private void OnEnable()
        {
            this.IsPinky = Random.Range(0, 10) < this._weaponConfig.ChanceOfBeingPinky;

            ParticleSystem.MainModule main = this._particleSystem.main;
            main.startColor = this.IsPinky ? this._pinkColor : this._originalColor;

            SFX.PlayProjectile(this._weaponConfig.SfxOnShoot);
            base.Invoke(nameof(this.Deactivate), this._weaponConfig.LifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            SFX.PlayExplosion(this._weaponConfig.SfxOnExplode);

            ParticleSystem particle = SpawnablePool.SpawnExplosion<ParticleSystem>(this._weaponConfig.ExplosionType);
            particle.transform.position = base.transform.position;

            ParticleSystem.MainModule main = particle.main;
            main.startColor = this._particleSystem.main.startColor;
            particle.gameObject.SetActive(true);

            this.Deactivate();
        }

        public void SetWeaponConfig(WeaponConfig weaponConfig)
        {
            this._weaponConfig = weaponConfig;
        }

        public void Shoot(Transform origin, Quaternion direction)
        {
            base.transform.position = origin.position;
            base.transform.rotation = direction;

            base.gameObject.SetActive(true);

            this._rigidbody.linearVelocity = origin.forward * this._weaponConfig.ProjectileSpeed;
        }

        private void Deactivate()
        {
            this._rigidbody.linearVelocity = Vector3.zero;
            // Automatically returns the object to the pool
            base.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            this.CancelInvoke();
        }
    }
}