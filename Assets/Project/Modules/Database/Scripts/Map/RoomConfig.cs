using System;
using System.Collections.Generic;

namespace Game.Database
{
    [Serializable]
    public class RoomConfig
    {
        public RoomGenerator Prefab;

        public bool IsUnique;
        public int MinRoomsBefore;

        public int MinEnemies = 1;
        public int MaxEnemies = 5;
        public List<SpawnTypeEnemy> EnemyPool = new();

        public int MinTraps = 1;
        public int MaxTraps = 5;
        public List<SpawnTypeTrap> TrapPool = new();

        public int MinPickupItems = 0;
        public int MaxPickupItems = 3;
        public List<SpawnTypePickup> PickupItemsPool = new();
    }
}
