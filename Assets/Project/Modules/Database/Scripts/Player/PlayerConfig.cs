using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class PlayerConfig : CharacterConfig, IPlayerConfig
    {
        [field: SerializeField] public LayerMask InvincibilityLayerMask { get; set; }
        [field: SerializeField] public float SpawnInvincibilityDuration { get; set; } = 3f;
        [field: SerializeField] public int InitialStatsPoints { get; set; } = 3;
        [field: SerializeField] public int StatsPointsPerLevel { get; set; } = 2;
        [field: SerializeField] public CharacterStats ExtraStatsPerPoint { get; set; }
        [field: SerializeField] public int[] MaxPointsForStats { get; set; } = new int[7];

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

        /**
         * 1 - Movement Speed
         * 2 - Dash Cooldown
         * 3 - Parry Cooldown
         * 4 - Max Health
         * 
         * returns (base stats, extra stats per point, maximum points for the stats)
         */
        public (float, float) GetStatsConfigByIndex(int index)
        {
            return index switch
            {
                1 => (base.Stats.MovementSpeed, this.ExtraStatsPerPoint.MovementSpeed),
                2 => (base.Stats.DashCooldown, this.ExtraStatsPerPoint.DashCooldown),
                3 => (base.Stats.ParryCooldown, this.ExtraStatsPerPoint.ParryCooldown),
                _ => (base.Stats.MaxHealth, this.ExtraStatsPerPoint.MaxHealth)
            };
        }

        public float GetMaxPointsForStatsByIndex(int index)
        {
            return index switch
            {
                1 =>  this.MaxPointsForStats[1],
                2 => this.MaxPointsForStats[4],
                3 => this.MaxPointsForStats[6],
                _ => this.MaxPointsForStats[0]
            };
        }
    }
}