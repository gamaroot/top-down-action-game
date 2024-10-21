using System;
using UnityEngine;
using Utils;

namespace Game.Database
{
    [Serializable]
    public class EnemyConfig : CharacterConfig, IEnemyConfig
    {
        [field: SerializeField] public SpawnTypeEnemy Type { get; set; }
        [field: SerializeField] public float XpReward { get; set; }

        // Sensor
        [field: SerializeField] public Tags TargetTag { get; set; } = Tags.Player;
        [field: SerializeField] public LayerMask ObstacleLayer { get; set; }
        [field: SerializeField] public float DetectionRadius { get; set; } = 20f;
    }

}