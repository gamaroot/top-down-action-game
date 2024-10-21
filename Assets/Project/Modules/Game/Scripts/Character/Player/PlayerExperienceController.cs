using Game.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerExperienceController : MonoBehaviour
    {
        [SerializeField] private Slider _xpBar;
        [SerializeField] private PlayerLevelUpDisplay _levelUpDisplay;

        private IGameManager _gameManager;

        public void Init(IGameManager gameManager)
        {
            this._gameManager = gameManager;

            this.AddExperience(0, false);
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            this.AddExperience(enemy.XpReward);
        }

        private void AddExperience(float xp, bool displayFeedback = true)
        {
            float currentXp = this._gameManager.GameState.PlayerState.XP;
            currentXp += xp;

            int currentLevel = this._gameManager.GameState.PlayerState.Level;
            float xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
            float remainingExperience = currentXp - xpToNextLevel;

            if (remainingExperience >= 0)
            {
                this._xpBar.value = 0;
                this._gameManager.OnPlayerLevelUp();
                this._gameManager.OnPlayerReceivedXp(remainingExperience);

                if (displayFeedback)
                    this.OnLevelUp();

                this.AddExperience(remainingExperience, displayFeedback);
            }
            else
            {
                this._xpBar.value = currentXp / xpToNextLevel;
                this._gameManager.OnPlayerReceivedXp(xp);
            }

        }

        private void OnLevelUp()
        {
            this._levelUpDisplay.OnShow();
        }
    }
}
