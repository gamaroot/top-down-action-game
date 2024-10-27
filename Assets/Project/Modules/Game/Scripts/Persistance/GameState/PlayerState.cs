using Game.Database;
using System;

namespace Game
{
    [Serializable]
    public class PlayerState
    {
        public int InitialLevel;
        public int Level;
        public float InitialXP;
        public float XP;

        public float CurrentHealth;
        public int[] ExtraStats;

        public float XpGained => this.XP - this.InitialXP;

        public void Init()
        {
            this.InitialXP = this.XP;
            this.InitialLevel = this.Level;
        }

        public CharacterStats GetStats(IPlayerConfig playerConfig)
        {
            return new CharacterStats
            {
                MaxHealth = playerConfig.Stats.MaxHealth + (this.ExtraStats[0] * playerConfig.ExtraStatsPerPoint.MaxHealth),
                MovementSpeed = playerConfig.Stats.MovementSpeed + (this.ExtraStats[1] * playerConfig.ExtraStatsPerPoint.MovementSpeed),
                DashSpeed = playerConfig.Stats.DashSpeed, // Not customizable by the player
                DashDuration = playerConfig.Stats.DashDuration, // Not customizable by the player
                DashCooldown = playerConfig.Stats.DashCooldown + (this.ExtraStats[2] * playerConfig.ExtraStatsPerPoint.DashCooldown),
                ParryDuration = playerConfig.Stats.ParryDuration, // Not customizable by the player
                ParryCooldown = playerConfig.Stats.ParryCooldown + (this.ExtraStats[3] * playerConfig.ExtraStatsPerPoint.ParryCooldown)
            };
        }
    }
}