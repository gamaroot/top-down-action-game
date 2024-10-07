using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class PlayerHealthController : HealthController
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _healthBar;

        private void Awake()
        {
            this._canvas.worldCamera = CameraHandler.Instance.MainCamera;
            this.HealthRecoverListener += this.OnHealthRecover;
            this.HealthLoseListener += this.OnHealthLose;
        }

        private void OnDisable()
        {
            CameraHandler.Instance.StopShake();
        }

        private void OnHealthRecover(float amount, float currentHealth, float maxHealth)
        {
            this._healthBar.value = currentHealth / maxHealth;
        }

        private void OnHealthLose(float amount, float currentHealth, float maxHealth)
        {
            this._healthBar.value = currentHealth / maxHealth;

            base.StartCoroutine(CameraHandler.Instance.Shake());
        }

        protected override void Die()
        {
            Destroy(base.gameObject);
        }
    }
}