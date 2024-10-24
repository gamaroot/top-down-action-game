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

            this.SetupUI();
        }

        private void SetupUI()
        {
            float currentXp = this._gameManager.GameState.PlayerState.XP;
            int currentLevel = this._gameManager.GameState.PlayerState.Level;

            float xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
            float remainingExperience = currentXp - xpToNextLevel;

            while (remainingExperience >= 0)
            {
                this._xpBar.value = 0;
                this._gameManager.OnPlayerLevelUp();

                // Update XP for the next loop iteration
                currentXp = remainingExperience;
                currentLevel = this._gameManager.GameState.PlayerState.Level;
                xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
                remainingExperience = currentXp - xpToNextLevel;
            }

            this._xpBar.value = currentXp / xpToNextLevel;
        }


        public void OnEnemyKill(IEnemyConfig enemy)
        {
            this.AddExperience(enemy.XpReward);
        }

        private void AddExperience(float xp)
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

                this.OnLevelUp();

                this.AddExperience(remainingExperience);
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
