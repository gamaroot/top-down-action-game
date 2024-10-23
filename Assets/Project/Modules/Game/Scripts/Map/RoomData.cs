using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RoomData
    {
        public int Id;
        public RoomGenerator Prefab;
        public float SquaredSize;
        public float WallHeight;
        public Vector2 Position;

        // 0: front, 1: back, 2: left, 3: right
        public int[] NeighborsID;

        public int TotalEnemies;
        public List<SpawnTypeEnemy> EnemyPool;

        public int TotalTraps;
        public List<SpawnTypeTrap> TrapPool;
    }
}
