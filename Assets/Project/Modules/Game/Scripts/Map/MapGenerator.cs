using Game.Database;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Game
{
    public class MapGenerator
    {
        private readonly Vector2[] _directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        private readonly Dictionary<Vector2, int> _occupiedPositions = new();

        private IRoom[] _rooms;

        public IRoom[] Generate(IMapConfig config, Transform parent)
        {
            this.DestroyMap(parent);

            var currentPosition = Vector2.zero;

            int totalRooms = Random.Range(config.MinRooms, config.MaxRooms);
            var roomsData = new RoomData[totalRooms];

            this._rooms = new Room[totalRooms];

            for (int index = 0; index < totalRooms; index++)
            {
                int totalDirections = 0;
                var possibleDirections = new List<Vector2>();

                // Check in each direction for existing neighbors
                for (int directionIndex = 0; directionIndex < this._directions.Length; directionIndex++)
                {
                    Vector2 direction = this._directions[directionIndex];
                    Vector2 neighborPosition = currentPosition + (direction * config.RoomSquaredSize);

                    // Check for neighbors
                    if (!this._occupiedPositions.ContainsKey(neighborPosition))
                    {
                        possibleDirections.Add(direction);
                        totalDirections++;
                    }
                }

                RoomConfig roomConfig = config.RoomConfigs[Random.Range(0, config.RoomConfigs.Count)];
                roomsData[index] = new RoomData
                {
                    Id = index,
                    Prefab = roomConfig.Prefab,
                    SquaredSize = config.RoomSquaredSize,
                    WallHeight = config.RoomWallHeight,
                    Position = currentPosition,
                    NeighborsID = new int[4] { -1, -1, -1, -1 },

                    TotalEnemies = Random.Range(roomConfig.MinEnemies, roomConfig.MaxEnemies),
                    EnemyPool = roomConfig.EnemyPool,

                    TotalTraps = Random.Range(roomConfig.MinTraps, roomConfig.MaxTraps),
                    TrapPool = roomConfig.TrapPool,
                };

                this._occupiedPositions.Add(currentPosition, index);

                currentPosition += totalDirections > 0
                                        ? possibleDirections[Random.Range(0, totalDirections)] * config.RoomSquaredSize
                                        : Vector2.zero;
            }

            for (int index = 0; index < totalRooms; index++)
            {
                var neighbors = new int[4];
                RoomData newRoomData = roomsData[index];
                currentPosition = newRoomData.Position;

                // Check in each direction for existing neighbors
                for (int directionIndex = 0; directionIndex < this._directions.Length; directionIndex++)
                {
                    Vector2 direction = this._directions[directionIndex];
                    Vector2 neighborPosition = currentPosition + (direction * config.RoomSquaredSize);

                    // Check for neighbors
                    if (this._occupiedPositions.ContainsKey(neighborPosition))
                    {
                        newRoomData.NeighborsID[directionIndex] = this._occupiedPositions[neighborPosition];
                    }
                }

                roomsData[index] = newRoomData;
                this.GenerateRoom(newRoomData, parent);
            }

            this.CreateNavMesh(parent);

            return this._rooms;
        }

        private void GenerateRoom(RoomData data, Transform parent)
        {
            RoomGenerator roomGenerator = MonoBehaviour.Instantiate(data.Prefab);
            roomGenerator.name = $"Room #{data.Id}";
            roomGenerator.transform.SetParent(parent);

            this._rooms[data.Id] = roomGenerator.Generate(data);
        }

        private void CreateNavMesh(Transform parent)
        {
            var navMesh = new GameObject("NavMeshSurface");
            navMesh.transform.SetParent(parent);
            navMesh.AddComponent<NavMeshSurface>().BuildNavMesh();

            for (int index = 0; index < this._rooms.Length; index++)
            {
                this._rooms[index].HideIfPlayerIsNotHere();
            }
        }

        private void DestroyMap(Transform parent)
        {
            for (int index = 0; index < parent.childCount; index++)
            {
                MonoBehaviour.Destroy(parent.GetChild(index).gameObject);
            }
        }
    }
}
