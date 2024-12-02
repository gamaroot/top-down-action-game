using Game.Database;
using System;

namespace Game
{
    public interface IGameManager
    {
        Action<int, int> OnPlayerHealthUpdateListener { get; set; }
        Action<float, float> OnPlayerXpUpdateListener { get; set; }
        Action<int> OnPlayerLevelUpdateListener { get; set; }

        IPlayerConfig PlayerConfig { get; }
        IEnemyConfig[] EnemyConfig { get; }
        IWeaponConfig[] WeaponConfig { get; }
        GameState GameState { get; }

        void OnPlayerStateUpdated(PlayerState playerStats);
        void OnPlayerHealthUpdated(int currenHealth, int maxHealth);
        void OnPlayerLoseHealth(float healthPercentage);
        void OnPlayerRecoverHealth(float healthPercentage);
        void OnPlayerXpUpdated(float xp, float xpToNextLevel);
        void OnPlayerLevelUp(float xpToNextLevel);
        void OnPlayerDeath();
        void OnPlayerEscape();
        void OnEnemyKill(IEnemyConfig enemy);
        void OnMapVisibilityChange(bool visible);
        void SetInputEnabled(bool inputEnabled);
    }
}
