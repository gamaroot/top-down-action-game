using System;
using UnityEngine;

namespace Game
{
    public abstract class HealthController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _maxHealth = 100f;

        public Action<float, float, float> HealthRecoverListener;
        public Action<float, float, float> HealthLoseListener;

        private float CurrentHealth
        {
            get => this._currentHealth; 
            set
            {
                this._currentHealth = Mathf.Clamp(value, 0, this._maxHealth);
            }
        }
        private float _currentHealth;

        private void OnEnable()
        {
            this.CurrentHealth = this._maxHealth;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out DamageDealer damageDealer))
            {
                this.TakeDamage(damageDealer.Damage);
            }
        }

        private void RecoverHealth(float amount)
        {
            this.CurrentHealth += amount;
            this.HealthRecoverListener.Invoke(amount, this.CurrentHealth, this._maxHealth);
        }

        private void TakeDamage(float amount)
        {
            this.CurrentHealth -= amount;
            this.HealthLoseListener.Invoke(amount, this.CurrentHealth, this._maxHealth);

            if (this.CurrentHealth <= 0)
                this.Die();
        }

        protected abstract void Die();
    }
}