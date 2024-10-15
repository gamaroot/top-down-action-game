using Game.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MapGenerator
    {
        private readonly Vector2[] _directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        private readonly HashSet<Vector2> _occupiedPositions = new();

        public void Generate(IMapConfig config, Action<RoomConfig> onRoomCreated, Action onComplete)
        {
            var currentPosition = Vector2.zero;

            int totalRooms = UnityEngine.Random.Range(config.MinRooms, config.MaxRooms);
            var rooms = new RoomConfig[totalRooms];

            for (int index = 0; index < totalRooms; index++)
            {
                var room = new RoomConfig
                {
                    Id = index,
                    Prefab = config.RoomPrefabs[UnityEngine.Random.Range(0, config.RoomPrefabs.Count)],
                    SquaredSize = config.RoomSquaredSize,
                    WallHeight = config.RoomWallHeight,
                    Position = currentPosition,
                    Neighbors = new bool[4]
                };

                var possibleDirections = new List<Vector2>();

                // Check in each direction for existing neighbors
                for (int directionIndex = 0; directionIndex < this._directions.Length; directionIndex++)
                {
                    Vector2 direction = this._directions[directionIndex];
                    Vector2 neighborPosition = room.Position + (direction * room.SquaredSize);

                    // Check for neighbors
                    if (this._occupiedPositions.Contains(neighborPosition))
                    {
                        room.Neighbors[directionIndex] = true;
                    }
                    else
                    {
                        possibleDirections.Add(direction);
                    }
                }

                rooms[index] = room;
                onRoomCreated.Invoke(room);

                this._occupiedPositions.Add(currentPosition);

                currentPosition += possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)] * room.SquaredSize;
            }

            onComplete.Invoke();
        }
    }
}
