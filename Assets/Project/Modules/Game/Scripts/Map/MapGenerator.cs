using Game.Database;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Game
{
    public class MapGenerator
    {
        private readonly Vector2[] _directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        private readonly Dictionary<Vector2, int> _occupiedPositions = new();
        private IRoom[] _rooms;

        public IRoom[] Generate(IMapConfig config, Transform parent)
        {
            this.DestroyMap(parent);

            int totalRooms = Random.Range(config.MinRooms, config.MaxRooms);
            var roomsData = new RoomData[totalRooms];
            var possibleRoomConfigs = new List<int>(config.RoomConfigs.Count);
            bool hasGeneratedAtLeastOneBossRoom = false;

            this._rooms = new Room[totalRooms];

            for (int index = 0; index < config.RoomConfigs.Count; index++)
                possibleRoomConfigs.Add(index);

            var currentPosition = Vector2.zero;

            for (int index = 0; index < totalRooms; index++)
            {
                var possibleDirections = this.GetAvailableDirections(currentPosition, config.RoomSquaredSize);

                int randomRoomConfig = Random.Range(0, possibleRoomConfigs.Count);
                RoomConfig roomConfig = config.RoomConfigs[randomRoomConfig];

                bool isLastRoom = index == totalRooms - 1;
                if (isLastRoom && !hasGeneratedAtLeastOneBossRoom)
                    roomConfig = config.RoomConfigs.Find(room => room.Prefab.Type == RoomType.BOSS);

                if (roomConfig.Prefab.Type == RoomType.BOSS && roomConfig.MinRoomsBefore > index)
                {
                    roomConfig = config.RoomConfigs[Random.Range(0, config.RoomConfigs.Count)];
                    hasGeneratedAtLeastOneBossRoom = true;
                }

                if (roomConfig.IsUnique)
                    possibleRoomConfigs.Remove(randomRoomConfig);

                roomsData[index] = this.CreateRoomData(index, roomConfig, currentPosition, config);
                this._occupiedPositions[currentPosition] = index;
                currentPosition += possibleDirections.Count > 0 ? 
                                            possibleDirections[Random.Range(0, possibleDirections.Count)] * config.RoomSquaredSize : 
                                            Vector2.zero;
            }

            for (int index = 0; index < totalRooms; index++)
            {
                this.UpdateNeighbors(roomsData[index], config.RoomSquaredSize);
                this.GenerateRoom(roomsData[index], parent);
            }

            this.CreateNavMesh(parent);
            return this._rooms;
        }

        private List<Vector2> GetAvailableDirections(Vector2 currentPosition, float roomSize)
        {
            var possibleDirections = new List<Vector2>();

            foreach (Vector2 direction in _directions)
            {
                Vector2 neighborPosition = currentPosition + (direction * roomSize);
                if (!_occupiedPositions.ContainsKey(neighborPosition))
                    possibleDirections.Add(direction);
            }

            return possibleDirections;
        }

        private RoomData CreateRoomData(int id, RoomConfig roomConfig, Vector2 position, IMapConfig config)
        {
            return new RoomData
            {
                Id = id,
                Prefab = roomConfig.Prefab,
                SquaredSize = config.RoomSquaredSize,
                WallHeight = config.RoomWallHeight,
                Position = position,
                NeighborsID = new int[4] { -1, -1, -1, -1 },
                TotalEnemies = Random.Range(roomConfig.MinEnemies, roomConfig.MaxEnemies),
                EnemyPool = roomConfig.EnemyPool,
                TotalTraps = Random.Range(roomConfig.MinTraps, roomConfig.MaxTraps),
                TrapPool = roomConfig.TrapPool
            };
        }

        private void UpdateNeighbors(RoomData roomData, float roomSize)
        {
            for (int index = 0; index < _directions.Length; index++)
            {
                Vector2 neighborPosition = roomData.Position + (this._directions[index] * roomSize);
                if (this._occupiedPositions.ContainsKey(neighborPosition))
                    roomData.NeighborsID[index] = this._occupiedPositions[neighborPosition];
            }
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

            foreach (IRoom room in this._rooms)
                room.HideIfPlayerIsNotHere();
        }

        private void DestroyMap(Transform parent)
        {
            for (int index = 0; index < parent.childCount; index++)
                MonoBehaviour.Destroy(parent.GetChild(index).gameObject);
        }
    }
}
