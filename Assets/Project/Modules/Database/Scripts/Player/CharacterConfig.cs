using System;

namespace Game.Database
{
    [Serializable]
    public class CharacterConfig
    {
        // Health
        public float MaxHealth = 10f;
        public SpawnTypeExplosion DeathVFX;
        public SFXTypeExplosion DeathSFX;

        // Movement
        public float MovementSpeed = 10f;
        public float DashSpeed = 30f;
        public float DashDuration = 0.2f;
        public float DashCooldown = 1f;

        // Parry
        public float ParryDuration = 0.3f;
        public float ParryCooldown = 1f;
    }
}