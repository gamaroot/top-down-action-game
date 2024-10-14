using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class CharacterConfig : ICharacterConfig
    {
        // Health
        [field: SerializeField] public float MaxHealth { get; set; } = 10f;
        [field: SerializeField] public SpawnTypeExplosion DeathVFX { get; set; }
        [field: SerializeField] public SFXTypeExplosion DeathSFX { get; set; }

        // Movement
        [field: SerializeField] public float MovementSpeed { get; set; } = 10f;
        [field: SerializeField] public float DashSpeed { get; set; } = 30f;
        [field: SerializeField] public float DashDuration { get; set; } = 0.2f;
        [field: SerializeField] public float DashCooldown { get; set; } = 1f;

        // Parry
        [field: SerializeField] public float ParryDuration { get; set; } = 0.3f;
        [field: SerializeField] public float ParryCooldown { get; set; } = 1f;
    }
}