using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class PlayerConfig : CharacterConfig, IPlayerConfig
    {
        [field: SerializeField] public int InitialStatsPoints { get; set; } = 3;
        [field: SerializeField] public int StatsPointsPerLevel { get; set; } = 2;
        [field: SerializeField] public CharacterStats ExtraStatsPerPoint { get; set; }

        [field: SerializeField] public float BaseLevelUpXp { get; set; } = 100f;
        [field: SerializeField] public float XpToNextLevelFactor { get; set; } = 1.1f;

        public float GetXpToNextLevel(int currentLevel)
        {
            return Mathf.Floor(this.BaseLevelUpXp * Mathf.Pow(this.XpToNextLevelFactor, currentLevel - 1));
        }

        public int GetTotalStatsPoints(int level)
        {
            return this.InitialStatsPoints + (this.StatsPointsPerLevel * (level - 1));
        }

        public float GetStatByIndex(int index)
        {
            return index switch
            {
                1 => base.Stats.MovementSpeed,
                2 => base.Stats.DashCooldown,
                3 => base.Stats.ParryCooldown,
                _ => base.Stats.MaxHealth,
            };
        }

        public float GetStatPerPointByIndex(int index)
        {
            return index switch
            {
                1 => this.ExtraStatsPerPoint.MovementSpeed,
                2 => this.ExtraStatsPerPoint.DashCooldown,
                3 => this.ExtraStatsPerPoint.ParryCooldown,
                _ => this.ExtraStatsPerPoint.MaxHealth,
            };
        }
    }
}