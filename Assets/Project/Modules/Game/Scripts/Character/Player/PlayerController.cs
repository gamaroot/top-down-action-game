using Game.Database;
using Unity.Cinemachine;
using UnityEngine;
using Utils;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerHealthController _healthController;
        [SerializeField] private PlayerExperienceController _experienceController;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerParryController _parryController;
        [SerializeField] private PlayerShootController _shootController;

        private int _killStreak;

        private void OnValidate()
        {
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

                this._healthController.Init(gameManager.PlayerConfig, stats);
                this._experienceController.Init(gameManager);
                this._movementController.Init(stats);
                this._parryController.Init(stats);
                this._shootController.Init(gameManager.WeaponConfig);
            }
            base.gameObject.gameObject.SetActive(isActive);
        }

        private void OnComboFinished()
        {
            this._killStreak = 0;
        }
    }
}
