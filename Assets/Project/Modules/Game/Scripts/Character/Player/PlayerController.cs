using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector3 _defaultPosition;
        [SerializeField] private PlayerHealthController _healthController;
        [SerializeField] private PlayerExperienceController _experienceController;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerParryController _parryController;
        [SerializeField] private PlayerShootController _shootController;

        public bool IsDead => this._healthController.IsDead;

        private int _killStreak;

        private void OnValidate()
        {
            if (this._defaultPosition == Vector3.zero)
                this._defaultPosition = base.transform.position;

            if (this._healthController == null)
                this._healthController = this.GetComponent<PlayerHealthController>();

            if (this._experienceController == null)
                this._experienceController = this.GetComponent<PlayerExperienceController>();

            if (this._movementController == null)
                this._movementController = this.GetComponent<PlayerMovementController>();

            if (this._parryController == null)
                this._parryController = this.GetComponent<PlayerParryController>();

            if (this._shootController == null)
                this._shootController = this.GetComponent<PlayerShootController>();
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            this._experienceController.OnEnemyKill(enemy);

            this._killStreak++;
            Statistics.Instance.OnEnemyKilled();
            Statistics.Instance.OnComboFinished(this._killStreak);

            base.Invoke(nameof(this.OnComboFinished), 3f);
        }

        public void Activate(bool isActive, IGameManager gameManager)
        {
            if (this == null)
                return;

            if (isActive)
            {
                CharacterStats stats = gameManager.GameState.PlayerState.GetStats(gameManager.PlayerConfig);

                this._healthController.Init(gameManager.PlayerConfig, stats, new CharacterHealthEvents
                {
                    OnLoseHealth = () => this.OnLoseHealth(gameManager),
                    OnRecoverHealth = () => this.OnRecoverHealth(gameManager),
                    OnDeath = () => gameManager.OnPlayerDeath(),
                });
                gameManager.OnPlayerHealthUpdated(this._healthController.MaxHealth, this._healthController.MaxHealth);

                this._experienceController.Init(gameManager);
                this._movementController.Init(stats);
                this._parryController.Init(stats);
                this._shootController.Init(gameManager.WeaponConfig);
            }
            base.gameObject.gameObject.SetActive(isActive);
        }

        public void ResetToDefaultPosition()
        {
            base.transform.position = this._defaultPosition;
        }

        public void OnRecoverHealth(IGameManager gameManager)
        {
            gameManager.OnPlayerHealthUpdated(this._healthController.CurrentHealth, this._healthController.MaxHealth);
        }

        private void OnComboFinished()
        {
            this._killStreak = 0;
        }

        private void OnLoseHealth(IGameManager gameManager)
        {
            gameManager.OnPlayerHealthUpdated(this._healthController.CurrentHealth, this._healthController.MaxHealth);

            if (base.gameObject.activeSelf)
                base.StartCoroutine(CameraHandler.Instance.Shake());
        }
    }
}
