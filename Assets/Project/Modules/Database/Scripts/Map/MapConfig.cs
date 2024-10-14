using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class MapConfig : IMapConfig
    {
        [field: SerializeField] public List<GameObject> RoomPrefabs { get; set; }
        [field: SerializeField] public int MinRooms { get; set; } = 10;
        [field: SerializeField] public int MaxRooms { get; set; } = 10;
    }
}