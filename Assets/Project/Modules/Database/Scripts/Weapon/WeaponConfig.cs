using System;

namespace Game.Database
{
    [Serializable]
    public class WeaponConfig
    {
        public WeaponType Type;
        public float Damage = 1f;
        public float ShootInterval = 0.3f;
        public float Range = 10f;
        public SFXTypeProjectile SfxOnShoot;
        public SFXTypeExplosion SfxOnExplode;
        public SpawnTypeExplosion ExplosionType;
        public float ProjectileSpeed = 20f;
        public float ChanceOfBeingPinky;
    }
}