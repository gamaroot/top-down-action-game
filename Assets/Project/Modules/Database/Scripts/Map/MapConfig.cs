using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class MapConfig : IMapConfig
    {
        [field: SerializeField] public List<RoomGenerator> RoomPrefabs { get; set; }
        [field: SerializeField] public int MinRooms { get; set; } = 10;
        [field: SerializeField] public int MaxRooms { get; set; } = 10;
        [field: SerializeField] public float RoomSquaredSize { get; set; }
        [field: SerializeField] public float RoomWallHeight { get; set; }
    }
}