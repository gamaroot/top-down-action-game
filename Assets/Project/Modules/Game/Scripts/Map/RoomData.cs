using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct RoomData
    {
        public int Id;
        public RoomGenerator Prefab;
        public float SquaredSize;
        public float WallHeight;
        public Vector2 Position;
        public bool[] Neighbors;

        public int TotalEnemies;
        public List<SpawnTypeEnemy> EnemyPool;

        public int TotalTraps;
        public List<SpawnTypeTrap> TrapPool;
    }
}
