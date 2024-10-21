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

        private GameObject[] _roomsMesh;

        public void Generate(IMapConfig config, Transform parent)
        {
            this.DestroyMap(parent);

            var currentPosition = Vector2.zero;

            int totalRooms = UnityEngine.Random.Range(config.MinRooms, config.MaxRooms);
            var rooms = new RoomData[totalRooms];

            this._roomsMesh = new GameObject[totalRooms];

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
        }

        private void GenerateRoom(RoomData room, Transform parent)
        {
            RoomGenerator roomGenerator = MonoBehaviour.Instantiate(room.Prefab);
            roomGenerator.name = $"Room #{room.Id}";
            roomGenerator.transform.SetParent(parent);

            var contentParent = new GameObject($"Content");
            contentParent.transform.SetParent(roomGenerator.transform);

            roomGenerator.Generate(room.SquaredSize, room.WallHeight, contentParent.transform, out List<GameObject> waypoints);
            roomGenerator.transform.position = new Vector3(room.Position.x, 0, room.Position.y);

            var spawnConfig = new SpawnerGenerator().GenerateSpawnConfig(room);

            roomGenerator.GetComponent<Room>().Init(room.Id, room.SquaredSize, room.Neighbors, waypoints, contentParent, spawnConfig);

            this._roomsMesh[room.Id] = contentParent;
        }

        private void CreateNavMesh(Transform parent)
        {
            var navMesh = new GameObject("NavMeshSurface");
            navMesh.transform.SetParent(parent);
            navMesh.AddComponent<NavMeshSurface>().BuildNavMesh();

            for (int index = 1; index < this._roomsMesh.Length; index++)
            {
                this._roomsMesh[index].SetActive(false);
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
