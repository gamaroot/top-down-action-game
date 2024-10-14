using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class WeaponConfig : IWeaponConfig
    {
        [field: SerializeField] public WeaponType Type { get; set; }
        [field: SerializeField] public float Damage { get; set; } = 1f;
        [field: SerializeField] public float ShootInterval { get; set; } = 0.3f;
        [field: SerializeField] public float Range { get; set; } = 10f;
        [field: SerializeField] public SFXTypeProjectile SfxOnShoot { get; set; }
        [field: SerializeField] public SFXTypeExplosion SfxOnExplode { get; set; }
        [field: SerializeField] public SpawnTypeExplosion ExplosionType { get; set; }
        [field: SerializeField] public float ProjectileSpeed { get; set; } = 20f;
        [field: SerializeField] public float ChanceOfBeingPinky { get; set; }
    }
}