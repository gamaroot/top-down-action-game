using UnityEngine;

namespace Game
{
    public struct RoomConfig
    {
        public int Id;
        public RoomGenerator Prefab;
        public float SquaredSize;
        public float WallHeight;
        public Vector2 Position;
        public bool[] Neighbors;
    }
}
