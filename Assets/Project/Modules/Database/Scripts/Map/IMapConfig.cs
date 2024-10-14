using System.Collections.Generic;
using UnityEngine;

namespace Game.Database
{
    public interface IMapConfig
    {
        List<GameObject> RoomPrefabs { get; }
        int MinRooms { get; }
        int MaxRooms { get; }
    }
}