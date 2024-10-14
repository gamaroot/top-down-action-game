using Game.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerExperienceController : MonoBehaviour
    {
        [SerializeField] private Slider _xpBar;
        [SerializeField] private PlayerLevelUpDisplay _levelUpDisplay;

        private float _xp = 1;
        private int _level = 1;
        private IPlayerConfig _config;

        public void Init(IPlayerConfig config)
        {
            this._config = config;
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            this.AddExperience(enemy.XpReward);
        }

        private void AddExperience(float xp)
        {
            this._xp += xp;
            float remainingExperience = this._xp - this.GetXpToNextLevel();

            if (remainingExperience >= 0)
            {
                this._xp = 0;
                this._xpBar.value = 0;

                this._level++;
                this.OnLevelUp();

                this.AddExperience(remainingExperience);
            }
            else
            {
                this._xpBar.value = this._xp / this.GetXpToNextLevel();
            }
        }

        private float GetXpToNextLevel()
        {
            return this._config.XpToNextLevel * Mathf.Pow(this._config.XpToNextLevelFactor, this._level - 1);
        }

        private void OnLevelUp()
        {
            this._levelUpDisplay.OnShow();
        }
    }
}
