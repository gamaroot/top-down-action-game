using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(ParticleSystem), typeof(Rigidbody), typeof(DamageDealer))]
    public class Bullet : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] private ParticleSystem _particleSystem;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField, ReadOnly] private DamageDealer _damageDealer;

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

            if (this._damageDealer == null)
                this._damageDealer = base.GetComponent<DamageDealer>();

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

        private void OnCollisionEnter(Collision collision)
        {
            ParryController parryController = collision.gameObject.GetComponent<ParryController>();
            if (parryController != null && parryController.IsParryActive)
                return;

            this.Deactivate();

            SFX.PlayExplosion(this._weaponConfig.SfxOnExplode);

            ParticleSystem particle = SpawnablePool.SpawnExplosion<ParticleSystem>(this._weaponConfig.ExplosionType);
            particle.transform.position = base.transform.position;

            ParticleSystem.MainModule main = particle.main;
            main.startColor = this._particleSystem.main.startColor;
            particle.gameObject.SetActive(true);
        }

        public void SetWeaponConfig(WeaponConfig weaponConfig)
        {
            this._weaponConfig = weaponConfig;
            this._damageDealer.LoadConfig(weaponConfig);
        }

        public void Shoot(Transform origin)
        {
            base.transform.position = origin.position;
            base.transform.rotation = Quaternion.LookRotation(origin.forward);

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