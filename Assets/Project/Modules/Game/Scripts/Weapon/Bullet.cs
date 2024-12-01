using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(ParticleSystem), typeof(Rigidbody), typeof(DamageDealer))]
    public class Bullet : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField, ReadOnly] private DamageDealer _damageDealer;

        private IWeaponConfig _config;

        private void OnValidate()
        {
            if (this._rigidbody == null)
                this._rigidbody = base.GetComponent<Rigidbody>();

            if (this._damageDealer == null)
                this._damageDealer = base.GetComponent<DamageDealer>();
        }

        private void OnEnable()
        {
            SFX.PlayBullet(this._config.Type);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ParryController parryController = collision.gameObject.GetComponent<ParryController>();
            if (parryController != null && parryController.IsParryActive)
                return;

            this.Deactivate();

            SFX.PlayBulletImpact(this._config.Type);

            GameObject vfx = SpawnablePool.SpawnBulletImpact(this._config.Type);
            vfx.transform.position = base.transform.position;
            vfx.gameObject.SetActive(true);
        }

        public void Setup(IWeaponConfig config)
        {
            this._config = config;
            this._damageDealer.SetDamage(config.Damage);
        }

        public void Shoot(Transform origin)
        {
            base.transform.position = origin.position;
            base.transform.rotation = Quaternion.LookRotation(origin.forward);

            base.gameObject.SetActive(true);

            this._rigidbody.linearVelocity = origin.forward * this._config.ProjectileSpeed;
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