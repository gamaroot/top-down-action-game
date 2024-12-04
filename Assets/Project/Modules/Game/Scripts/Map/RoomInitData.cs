using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct RoomInitData
    {
        public int Id;
        public RoomType Type;
        public GameObject[] Doors;
        public List<GameObject> Waypoints;
        public GameObject Content;
        public Tuple<SpawnConfig<SpawnTypeEnemy>[], 
                     SpawnConfig<SpawnTypeTrap>[], 
                     SpawnConfig<SpawnTypePickup>[]> SpawnConfig;
    }
}
