using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class HealthController : MonoBehaviour
    {
        public CharacterHealthEvents Listener = new();

        public int MissingHealth => this.MaxHealth - this.CurrentHealth;
        public bool IsFullHealth => this.CurrentHealth == this.MaxHealth;
        public bool IsDead => this.CurrentHealth <= 0;

        public virtual int MaxHealth => this._config.Stats.MaxHealth;

        public int CurrentHealth
        {
            get => this._currentHealth; 
            set
            {
                this._currentHealth = Mathf.Clamp(value, 0, this.MaxHealth);
            }
        }
        private int _currentHealth;

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

        protected void OnReset()
        {
            this.CurrentHealth = this.MaxHealth;
            base.gameObject.SetActive(true);
        }

        protected void RecoverHealth(int amount)
        {
            if (amount <= 0)
                return;

            if (amount > this.MissingHealth)
                amount = this.MissingHealth;

            this.CurrentHealth += amount;

            this.ShowHealthText(amount.ToString(), Color.green);

            this.Listener.OnRecoverHealth?.Invoke();
        }

        protected virtual void TakeDamage(int amount)
        {
            this.CurrentHealth -= amount;

            this.ShowHealthText(amount.ToString(), Color.red);

            this.Listener.OnLoseHealth?.Invoke();

            if (this.CurrentHealth <= 0)
                this.OnDeath();
        }

        public void OnDeath(bool hasSelfDestroyed = false)
        {
            if (!base.gameObject.activeSelf)
                return;

            base.gameObject.SetActive(false);

            if (hasSelfDestroyed)
                this.Listener.OnSelfDestroy?.Invoke();
            else
                this.Listener.OnDeath?.Invoke();

            SFX.PlayExplosion(this._config.DeathSFX);

            ParticleSystem explosion = SpawnablePool.SpawnExplosion<ParticleSystem>(this._config.DeathVFX);
            explosion.transform.position = base.transform.position;
            explosion.gameObject.SetActive(true);
        }

        protected void OnRespawn()
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