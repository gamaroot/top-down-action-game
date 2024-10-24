﻿using System;

namespace Game.Database
{
    [Serializable]
    public struct CharacterStats
    {
        public float MaxHealth;

        // Movement
        public float MovementSpeed;
        public float DashSpeed;
        public float DashDuration;
        public float DashCooldown;

        // Parry
        public float ParryDuration;
        public float ParryCooldown;
    }
}
