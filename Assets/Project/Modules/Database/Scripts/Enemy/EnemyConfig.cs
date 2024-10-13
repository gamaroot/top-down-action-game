using System;
using UnityEngine;
using Utils;

namespace Game.Database
{
    [Serializable]
    public class EnemyConfig : CharacterConfig
    {
        public SpawnTypeEnemy Type;

        // Sensor
        public Tags TargetTag = Tags.Player;
        public LayerMask ObstacleLayer;
        public float DetectionRadius = 20f;

        // Movement
        public float DashPossibility = 0.2f;

        // Parry
        public float ParryPossibility = 0.2f;
    }

}