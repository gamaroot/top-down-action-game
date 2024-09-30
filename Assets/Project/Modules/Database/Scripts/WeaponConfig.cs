using UnityEngine;

namespace Game.Database
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Game/WeaponConfig", order = 1)]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Weapon Stats")]
        [field: SerializeField] public float Damage { get; private set; } = 10f;
        [field: SerializeField] public float FireRate { get; private set; } = 0.5f;      // Time between shots (in seconds)
        [field: SerializeField] public float Range { get; private set; } = 50f;          // How far the weapon can shoot
        [field: SerializeField] public int AmmoCapacity { get; private set; } = 30;      // Max ammo capacity

        [Header("SFX")]
        [field: SerializeField] public SFXType SfxOnShoot { get; private set; }
        [field: SerializeField] public SFXType SfxOnExplode { get; private set; }

        [Header("Explosion")]
        [field: SerializeField] public SpawnType ExplosionType { get; private set; }

        [Header("Bullet Settings")]
        [field: SerializeField] public GameObject BulletPrefab { get; private set; }     // Bullet prefab to spawn when firing
        [field: SerializeField] public float BulletSpeed { get; private set; } = 20f;    // Speed of the bullet
        [field: SerializeField] public float LifeTime { get; private set; } = 10f;       // Time before the bullet is destroyed
        [Range(0, 10)]
        [field: SerializeField] public int ChanceOfBeingPinky { get; private set; } = 1; // Pink bullets can be parried
    }
}