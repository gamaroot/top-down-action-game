using Game.Database;

namespace Game
{
    public interface IGameManager
    {
        IPlayerConfig PlayerConfig { get; }
        IEnemyConfig[] EnemyConfig { get; }
        IWeaponConfig[] WeaponConfig { get; }
        void OnPlayerDeath();
        void OnPlayerRespawn();
        void OnEnemyKill(IEnemyConfig enemy);
    }
}
