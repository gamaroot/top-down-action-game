using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class HealthController : MonoBehaviour
    {
        public float MissingHealth => this.MaxHealth - this.CurrentHealth;
        public bool IsFullHealth => this.CurrentHealth == this.MaxHealth;
        public bool IsDead => this.CurrentHealth <= 0;

        public float MaxHealth => this._config.MaxHealth;

        public float CurrentHealth
        {
            get => this._currentHealth; 
            set
            {
                this._currentHealth = Mathf.Clamp(value, 0, this.MaxHealth);
            }
        }
        private float _currentHealth;

        private ICharacterConfig _config;

        private float _lastDamageDealerID;

        private void OnEnable()
        {
            this.CurrentHealth = this.MaxHealth;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DamageDealer damageDealer) &&
                damageDealer.ID != this._lastDamageDealerID)
            {
                this._lastDamageDealerID = damageDealer.ID;
                this.TakeDamage(damageDealer.Damage);
            }
        }

        public void Init(ICharacterConfig config)
        {
            this._config = config;
        }

        public virtual void OnReset()
        {
            this.CurrentHealth = this.MaxHealth;
            base.gameObject.SetActive(true);
        }

        public virtual void RecoverHealth(float amount)
        {
            if (amount <= 0)
                return;

            if (amount > this.MissingHealth)
                amount = this.MissingHealth;

            this.CurrentHealth += amount;

            this.ShowHealthText(amount.ToString(), Color.green);
        }

        protected virtual void TakeDamage(float amount)
        {
            this.CurrentHealth -= amount;

            this.ShowHealthText(amount.ToString(), Color.red);

            if (this.CurrentHealth <= 0)
                this.OnDeath();
        }

        public virtual void OnDeath()
        {
            if (!base.gameObject.activeSelf)
                return;

            base.gameObject.SetActive(false);

            SFX.PlayExplosion(this._config.DeathSFX);

            ParticleSystem explosion = SpawnablePool.SpawnExplosion<ParticleSystem>(this._config.DeathVFX);
            explosion.transform.position = base.transform.position;
            explosion.gameObject.SetActive(true);
        }

        public virtual void OnRespawn()
        {
            this.RecoverHealth(this.MaxHealth);
            base.gameObject.SetActive(true);
        }

        private void ShowHealthText(string amount, Color color)
        {
            UIWorldJumpingText text = SpawnablePool.SpawnOther<UIWorldJumpingText>(SpawnTypeOther.WORLD_JUMPING_TEXT);
            text.SetText(amount, color);
            text.gameObject.transform.position = base.transform.position;
            text.gameObject.SetActive(true);
        }
    }
}