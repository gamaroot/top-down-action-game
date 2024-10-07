using UnityEngine;
using Utils;

namespace Game
{
    public class ParryController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _cooldown = 1f;

        [Header("Components")]
        [SerializeField] private GameObject _projectileDeflector;

        public bool IsParryActive => this._projectileDeflector.activeSelf;

        private bool CanParry => Time.time - this._lastParryTime > this._cooldown;

        private InputController _inputs;
        private float _lastParryTime;

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

        private void Parry()
        {
            if (!this.CanParry)
                return;

            this._lastParryTime = Time.time;
            this._projectileDeflector.SetActive(true);

            base.Invoke(nameof(this.DeactivateDeflector), this._duration);
        }

        private void DeactivateDeflector()
        {
            this._projectileDeflector.SetActive(false);
        }
    }
}