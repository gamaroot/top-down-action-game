using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class PlayerHealthController : HealthController
    {
        [SerializeField] private Slider _hpBar;

        private void OnDisable()
        {
            CameraHandler.Instance.StopShake();
        }

        public override void RecoverHealth(float amount)
        {
            base.RecoverHealth(amount);
            this._hpBar.value = base.CurrentHealth / base.MaxHealth;
        }

        protected override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            this._hpBar.value = base.CurrentHealth / base.MaxHealth;

            if (base.gameObject.activeSelf)
                base.StartCoroutine(CameraHandler.Instance.Shake());
        }

        public override void OnReset()
        {
            base.OnReset();
            this._hpBar.value = base.CurrentHealth / base.MaxHealth;
        }
    }
}