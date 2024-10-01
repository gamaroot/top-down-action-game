using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class WeaponConfig
    {
        [field: SerializeField] public WeaponType Type { get; private set; }
        [field: SerializeField] public float Damage { get; private set; } = 10f;

        [Tooltip("Time between shots (in seconds)")]
        [field: SerializeField] public float ShootInterval { get; private set; } = 0.5f;

        [Tooltip("How far the weapon can shoot")]
        [field: SerializeField] public float Range { get; private set; } = 50f;

        [Tooltip("Max ammo capacity")]
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 30;

        [field: SerializeField] public SFXTypeProjectile SfxOnShoot { get; private set; }
        [field: SerializeField] public SFXTypeExplosion SfxOnExplode { get; private set; }

        [field: SerializeField] public SpawnTypeExplosion ExplosionType { get; private set; }

        [Tooltip("Speed of the bullet")]
        [field: SerializeField] public float ProjectileSpeed { get; private set; } = 20f;

        [Tooltip("Time before the bullet is destroyed")]
        [field: SerializeField] public float LifeTime { get; private set; } = 10f;

        [Tooltip("from 0 to 10, the chance to spawn bullets that can be parried")]
        [Range(0, 10)]
        [field: SerializeField] public int ChanceOfBeingPinky { get; private set; } = 1;
    }
}