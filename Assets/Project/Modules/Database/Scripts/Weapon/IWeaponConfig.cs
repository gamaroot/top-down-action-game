namespace Game.Database
{
    public interface IWeaponConfig
    {
        WeaponType Type { get; }
        int Damage { get; }
        float ShootInterval { get; }
        float Range { get; }
        SFXTypeProjectile SfxOnShoot { get; }
        SFXTypeExplosion SfxOnExplode { get; }
        SpawnTypeExplosion ExplosionType { get; }
        float ProjectileSpeed { get; }
        float ChanceOfBeingPinky { get; }
    }
}
