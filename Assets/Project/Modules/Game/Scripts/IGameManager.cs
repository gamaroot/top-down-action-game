using Game.Database;

namespace Game
{
    public interface IGameManager
    {
        IPlayerConfig PlayerConfig { get; }
        IEnemyConfig[] EnemyConfig { get; }
        IWeaponConfig[] WeaponConfig { get; }
        GameState GameState { get; }

        void OnPlayerStateUpdate(PlayerState playerStats);
        void OnPlayerReceivedXp(float xp);
        void OnPlayerLevelUp();
        void OnPlayerDeath();
        void OnPlayerRespawn();
        void OnEnemyKill(IEnemyConfig enemy);
        void OnMapVisibilityChange(bool visible);
    }
}
