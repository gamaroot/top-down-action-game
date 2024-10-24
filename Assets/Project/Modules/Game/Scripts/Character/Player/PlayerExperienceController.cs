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
            this.UpdateXp(this._gameManager.GameState.PlayerState.XP, this._gameManager.GameState.PlayerState.Level);
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            Debug.Log($"Received XP [{enemy.Type}]: {enemy.XpReward}");
            this.AddExperience(enemy.XpReward);
        }

        private void AddExperience(float xp)
        {
            this._gameManager.OnPlayerReceivedXp(xp);
            this.UpdateXp(this._gameManager.GameState.PlayerState.XP, this._gameManager.GameState.PlayerState.Level);
        }

        private void UpdateXp(float currentXp, int currentLevel)
        {
            float xpToNextLevel = _gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
            float remainingExperience = currentXp - xpToNextLevel;

            while (remainingExperience >= 0)
            {
                this._xpBar.value = 0;

                this._levelUpDisplay.OnShow();
                this._gameManager.OnPlayerLevelUp();

                currentXp = remainingExperience;
                currentLevel = this._gameManager.GameState.PlayerState.Level;
                xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
                remainingExperience = currentXp - xpToNextLevel;
            }

            this._xpBar.value = currentXp / xpToNextLevel;
        }
    }
}
