using UnityEngine;
using Utils;

namespace Game.Database
{
    public interface IEnemyConfig : ICharacterConfig
    {
        SpawnTypeEnemy Type { get; }
        float XpReward { get; }
        Tags TargetTag { get; }
        LayerMask ObstacleLayer { get; }
        float DetectionRadius { get; }
    }
}
