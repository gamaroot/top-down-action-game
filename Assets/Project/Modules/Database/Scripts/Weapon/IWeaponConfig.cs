namespace Game.Database
{
    public interface IWeaponConfig
    {
        WeaponType Type { get; }
        int Damage { get; }
        float ShootInterval { get; }
        float Range { get; }
        float ProjectileSpeed { get; }
    }
}
