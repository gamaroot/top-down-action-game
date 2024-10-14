using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class PlayerConfig : CharacterConfig, IPlayerConfig
    {
        [field: SerializeField] public int StatsPointsPerLevel { get; set; } = 3;
        [field: SerializeField] public float XpToNextLevel { get; set; } = 100f;
        [field: SerializeField] public float XpToNextLevelFactor { get; set; } = 1.1f;
    }

}