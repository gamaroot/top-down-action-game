using System.Collections.Generic;

namespace Game.Database
{
    public interface IMapConfig
    {
        int MinRooms { get; }
        int MaxRooms { get; }
        float RoomSquaredSize { get; }
        float RoomWallHeight { get; }

        List<RoomConfig> RoomConfigs { get; set; }
    }
}