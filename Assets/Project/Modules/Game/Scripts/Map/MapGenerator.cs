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
            bool hasGeneratedBossRoom = false;

            for (int index = 0; index < config.RoomConfigs.Count; index++)
            {
                if (config.RoomConfigs[index].MinRoomsBefore == 0)
                    possibleRoomConfigs.Add(index);
            }

            this._rooms = new Room[totalRooms];
            Vector2 currentPosition = Vector2.zero;

            // Generate room data
            for (int index = 0; index < totalRooms; index++)
            {
                List<Vector2> availableDirections = this.GetAvailableDirections(currentPosition, config.RoomSquaredSize);
                RoomConfig roomConfig = this.SelectRoomConfig(config, possibleRoomConfigs, index, totalRooms, ref hasGeneratedBossRoom);

                // Mark unique rooms to avoid reuse
                if (roomConfig.IsUnique)
                    possibleRoomConfigs.Remove(config.RoomConfigs.IndexOf(roomConfig));

                roomsData[index] = this.CreateRoomData(index, roomConfig, currentPosition, config);
                this._occupiedPositions[currentPosition] = index;

                currentPosition += availableDirections.Count > 0
                                        ? availableDirections[Random.Range(0, availableDirections.Count)] * config.RoomSquaredSize
                                        : Vector2.zero;

                this.UpdateAvailableConfigs(config, possibleRoomConfigs, index);
            }

            // Generate and place rooms
            for (int index = 0; index < totalRooms; index++)
            {
                this.UpdateNeighbors(roomsData[index], config.RoomSquaredSize);
                this.GenerateRoom(roomsData[index], parent);
            }

            // Finalize by creating navigation mesh
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
                TotalHealthItems = Random.Range(roomConfig.MinHealthItems, roomConfig.MaxHealthItems),
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

        private RoomConfig SelectRoomConfig(IMapConfig config, List<int> possibleConfigs, int currentIndex, int totalRooms, ref bool hasGeneratedBossRoom)
        {
            if (currentIndex == totalRooms - 1)
                return config.RoomConfigs[config.RoomConfigs.Count - 1];

            if (currentIndex == totalRooms - 2 && !hasGeneratedBossRoom)
            {
                hasGeneratedBossRoom = true;
                return config.RoomConfigs.Find(rc => rc.Prefab.Type == RoomType.BOSS);
            }

            int randomIndex = Random.Range(0, possibleConfigs.Count);
            return config.RoomConfigs[possibleConfigs[randomIndex]];
        }

        private void UpdateAvailableConfigs(IMapConfig config, List<int> possibleConfigs, int currentIndex)
        {
            for (int index = 0; index < config.RoomConfigs.Count; index++)
            {
                if (!possibleConfigs.Contains(index) && config.RoomConfigs[index].MinRoomsBefore == currentIndex + 1)
                {
                    possibleConfigs.Add(index);
                }
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
