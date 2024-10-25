using Game.Database;
using UnityEngine;

namespace Game
{
    public class PlayerExperienceController : MonoBehaviour
    {
        [SerializeField] private PlayerLevelUpDisplay _levelUpDisplay;

        private IGameManager _gameManager;

        private float xpToNextLevel;

        public void Init(IGameManager gameManager)
        {
            this._gameManager = gameManager;

            int currentLevel = this._gameManager.GameState.PlayerState.Level;
            this.xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);

            this.UpdateXp(this._gameManager.GameState.PlayerState.XP);
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            Debug.Log($"Received XP [{enemy.Type}]: {enemy.XpReward}");
            this.AddExperience(enemy.XpReward);
        }

        private void AddExperience(float xp)
        {
            this.UpdateXp(this._gameManager.GameState.PlayerState.XP + xp);
        }

        private void UpdateXp(float currentXp)
        {
            float remainingExperience = currentXp - this.xpToNextLevel;

            if (remainingExperience < 0)
            {
                this._gameManager.OnPlayerXpUpdated(currentXp, this.xpToNextLevel);
                return;
            }

            do
            {
                currentXp = remainingExperience;
                int currentLevel = this._gameManager.GameState.PlayerState.Level;
                this.xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(currentLevel);
                remainingExperience = currentXp - this.xpToNextLevel;

                this._levelUpDisplay.OnShow();
                this._gameManager.OnPlayerLevelUp(this.xpToNextLevel);
            } 
            while (remainingExperience >= 0);

            this._gameManager.OnPlayerXpUpdated(currentXp, this.xpToNextLevel);
        }
    }
}
