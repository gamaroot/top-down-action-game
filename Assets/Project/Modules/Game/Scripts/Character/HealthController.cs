using System;
using UnityEngine;
using Utils;

namespace Game
{
    public class HealthController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _maxHealth = 10f;
        [SerializeField] private SpawnTypeExplosion _deathVFX;
        [SerializeField] private SFXTypeExplosion _deathSFX;

        public Action<float, float, float> HealthRecoverListener;
        public Action<float, float, float> HealthLoseListener;

        public float MissingHealth => this._maxHealth - this.CurrentHealth;
        public bool IsFullHealth => this.CurrentHealth == this._maxHealth;
        public bool IsDead => this.CurrentHealth <= 0;

        private float CurrentHealth
        {
            get => this._currentHealth; 
            set
            {
                this._currentHealth = Mathf.Clamp(value, 0, this._maxHealth);
            }
        }
        private float _currentHealth;

        private float _lastDamageDealerID;

        private void OnEnable()
        {
            this.CurrentHealth = this._maxHealth;
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

        public void Respawn()
        {
            this.RecoverHealth(this._maxHealth);
            base.gameObject.SetActive(true);
        }

        public void RecoverHealth(float amount)
        {
            if (amount <= 0)
                return;

            if (amount > this.MissingHealth)
                amount = this.MissingHealth;

            this.CurrentHealth += amount;
            this.HealthRecoverListener.Invoke(amount, this.CurrentHealth, this._maxHealth);

            this.ShowHealthText(amount.ToString(), Color.green);
        }

        private void TakeDamage(float amount)
        {
            this.CurrentHealth -= amount;
            this.HealthLoseListener.Invoke(amount, this.CurrentHealth, this._maxHealth);

            this.ShowHealthText(amount.ToString(), Color.red);

            if (this.CurrentHealth <= 0)
                this.OnDeath();
        }

        public virtual void OnDeath()
        {
            if (!base.gameObject.activeSelf)
                return;

            base.gameObject.SetActive(false);

            SFX.PlayExplosion(this._deathSFX);

            ParticleSystem explosion = SpawnablePool.SpawnExplosion<ParticleSystem>(this._deathVFX);
            explosion.transform.position = base.transform.position;
            explosion.gameObject.SetActive(true);
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