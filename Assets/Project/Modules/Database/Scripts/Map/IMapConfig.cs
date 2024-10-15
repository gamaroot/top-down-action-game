using System.Collections.Generic;

namespace Game.Database
{
    public interface IMapConfig
    {
        List<RoomGenerator> RoomPrefabs { get; }
        int MinRooms { get; }
        int MaxRooms { get; }
        float RoomSquaredSize { get; }
        float RoomWallHeight { get; }
    }
}