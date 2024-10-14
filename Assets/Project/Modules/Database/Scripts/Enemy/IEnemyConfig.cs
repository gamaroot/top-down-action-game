using UnityEngine;
using Utils;

namespace Game.Database
{
    public interface IEnemyConfig : ICharacterConfig
    {
        SpawnTypeEnemy Type { get; }
        Tags TargetTag { get; }
        LayerMask ObstacleLayer { get; }
        float DetectionRadius { get; }
        float DashPossibility { get; }
        float ParryPossibility { get; }
    }
}
