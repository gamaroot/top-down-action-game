using Game.Database;
using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Game
{
    public class MapGenerator
    {
        private readonly Vector2[] _directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        private readonly HashSet<Vector2> _occupiedPositions = new();

        private IRoom[] _rooms;

        public IRoom[] Generate(IMapConfig config, Transform parent)
        {
            this.DestroyMap(parent);

            var currentPosition = Vector2.zero;

            int totalRooms = UnityEngine.Random.Range(config.MinRooms, config.MaxRooms);
            var rooms = new RoomData[totalRooms];

            this._rooms = new Room[totalRooms];

            for (int index = 0; index < totalRooms; index++)
            {
                RoomConfig roomConfig = config.RoomConfigs[UnityEngine.Random.Range(0, config.RoomConfigs.Count)];

                var room = new RoomData
                {
                    Id = index,
                    Prefab = roomConfig.Prefab,
                    SquaredSize = config.RoomSquaredSize,
                    WallHeight = config.RoomWallHeight,
                    Position = currentPosition,
                    Neighbors = new bool[4],

                    TotalEnemies = UnityEngine.Random.Range(roomConfig.MinEnemies, roomConfig.MaxEnemies),
                    EnemyPool = roomConfig.EnemyPool,

                    TotalTraps = UnityEngine.Random.Range(roomConfig.MinTraps, roomConfig.MaxTraps),
                    TrapPool = roomConfig.TrapPool,
                };

                var possibleDirections = new List<Vector2>();
                int totalDirections = 0;

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
                        totalDirections++;
                    }
                }

                rooms[index] = room;
                this.GenerateRoom(room, parent);

                this._occupiedPositions.Add(currentPosition);

                currentPosition += totalDirections > 0
                                        ? possibleDirections[UnityEngine.Random.Range(0, totalDirections)] * room.SquaredSize
                                        : Vector2.zero;
            }

            this.CreateNavMesh(parent);

            return this._rooms;
        }

        private void GenerateRoom(RoomData data, Transform parent)
        {
            RoomGenerator roomGenerator = MonoBehaviour.Instantiate(data.Prefab);
            roomGenerator.name = $"Room #{data.Id}";
            roomGenerator.transform.SetParent(parent);

            var contentParent = new GameObject($"Content");
            contentParent.transform.SetParent(roomGenerator.transform);

            roomGenerator.Generate(data.SquaredSize, data.WallHeight, contentParent.transform, out List<GameObject> waypoints);
            roomGenerator.transform.position = new Vector3(data.Position.x, 0, data.Position.y);

            var spawnConfig = new SpawnerGenerator().GenerateSpawnConfig(data);

            Room room = roomGenerator.GetComponent<Room>();
            room.Init(data.Id, data.SquaredSize, data.Neighbors, waypoints, contentParent, spawnConfig);

            this._rooms[data.Id] = room;
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
