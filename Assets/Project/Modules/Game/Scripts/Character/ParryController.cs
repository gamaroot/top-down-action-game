using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public abstract class ParryController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] private GameObject _projectileDeflector;

        public bool IsParryActive => this._projectileDeflector.activeSelf;
        private bool CanParry => Time.time - this._lastParryTime > this.GetCooldown();

        private InputController _inputs;
        private float _lastParryTime;

        private void OnValidate()
        {
            if (this._projectileDeflector == null)
                this._projectileDeflector = base.GetComponentInChildren<ProjectileDeflector>().gameObject;
        }

        private void Awake()
        {
            this._inputs = new InputController();
            this._inputs.Player.Parry.performed += _ => this.Parry();
        }

        private void OnEnable()
        {
            this._inputs.Enable();
        }

        private void OnDisable()
        {
            this._inputs.Disable();
            base.CancelInvoke();
        }

        public abstract float GetCooldown();
        public abstract float GetDuration();

        private void Parry()
        {
            if (!this.CanParry)
                return;

            this._lastParryTime = Time.time;
            this._projectileDeflector.SetActive(true);

            base.Invoke(nameof(this.DeactivateDeflector), this.GetDuration());
        }

        private void DeactivateDeflector()
        {
            this._projectileDeflector.SetActive(false);
        }
    }
}