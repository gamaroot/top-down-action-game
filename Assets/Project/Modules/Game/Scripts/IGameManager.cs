using Game.Database;

namespace Game
{
    public interface IGameManager
    {
        ICharacterConfig PlayerConfig { get; }
        IEnemyConfig[] EnemyConfig { get; }
        IWeaponConfig[] WeaponConfig { get; }
    }
}
