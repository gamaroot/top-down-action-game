using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class PlayerHealthController : HealthController
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private ToastHandler _toastHandler;

        private void Awake()
        {
            this._canvas.worldCamera = CameraHandler.Instance.MainCamera;
        }

        private void OnDisable()
        {
            CameraHandler.Instance.StopShake();
        }

        public override void RecoverHealth(float amount)
        {
            base.RecoverHealth(amount);
            this._healthBar.value = base.CurrentHealth / base.MaxHealth;
        }

        protected override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            this._healthBar.value = base.CurrentHealth / base.MaxHealth;

            if (base.gameObject.activeSelf)
                base.StartCoroutine(CameraHandler.Instance.Shake());
        }

        public override void OnRespawn()
        {
            base.OnRespawn();
            this._toastHandler.Show("You Respawned!");
        }

        public override void OnDeath()
        {
            base.OnDeath();

            this._toastHandler.Show("You died!");
        }

        public override void OnReset()
        {
            base.OnReset();
            this._healthBar.value = base.CurrentHealth / base.MaxHealth;
        }
    }
}