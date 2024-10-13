using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class ParryController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField, ReadOnly] private float _duration;
        [SerializeField, ReadOnly] private float _cooldown;

        [Header("Components")]
        [SerializeField, ReadOnly] private GameObject _projectileDeflector;

        public bool IsParryActive => this._projectileDeflector.activeSelf;

        private bool CanParry => Time.time - this._lastParryTime > this._cooldown;

        private InputController _inputs;
        private float _lastParryTime;

        private void OnValidate()
        {
            if (this._projectileDeflector == null)
                this._projectileDeflector = base.GetComponentInChildren<ProjectileDeflector>().gameObject;

            this.LoadConfig();
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

        private void LoadConfig()
        {
            if (base.gameObject.CompareTag(GameTags.PLAYER))
                this.LoadConfig<CharacterConfig>(ProjectPaths.PLAYER_CONFIG_DATABASE);
            else
                this.LoadConfig<EnemyConfig>(ProjectPaths.ENEMY_CONFIG_DATABASE);
        }

        private void LoadConfig<T>(string databasePath) where T : CharacterConfig
        {
            var database = Resources.Load<CharacterConfigDatabase<T>>(databasePath);
            if (database != null)
            {
                this._duration = database.Config.MovementSpeed;
                this._cooldown = database.Config.DashSpeed;
            }
        }
    }
}