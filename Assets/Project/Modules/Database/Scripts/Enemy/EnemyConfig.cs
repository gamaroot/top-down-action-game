using System;
using UnityEngine;
using Utils;

namespace Game.Database
{
    [Serializable]
    public class EnemyConfig : CharacterConfig, IEnemyConfig
    {
        [field: SerializeField] public SpawnTypeEnemy Type { get; set; }

        // Sensor
        [field: SerializeField] public Tags TargetTag { get; set; } = Tags.Player;
        [field: SerializeField] public LayerMask ObstacleLayer { get; set; }
        [field: SerializeField] public float DetectionRadius { get; set; } = 20f;

        // Movement
        [field: SerializeField] public float DashPossibility { get; set; } = 0.2f;

        // Parry
        [field: SerializeField] public float ParryPossibility { get; set; } = 0.2f;
    }

}