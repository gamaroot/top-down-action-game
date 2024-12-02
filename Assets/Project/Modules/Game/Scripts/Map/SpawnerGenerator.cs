using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnerGenerator
    {
        public Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[], SpawnConfig<SpawnTypePickup>[]> GenerateSpawnConfig(RoomData config)
        {
            var enemies = this.GenerateSpawnConfigForType(config.TotalEnemies, config.EnemyPool, 
                                                          config.SquaredSize, config.Position);

            var traps = this.GenerateSpawnConfigForType(config.TotalTraps, config.TrapPool, 
                                                        config.SquaredSize, config.Position);

            var pickups = this.GenerateSpawnConfigForType(config.TotalPickups, config.PickupsPool,
                                                        config.SquaredSize, config.Position);

            return new Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[], SpawnConfig<SpawnTypePickup>[]>(enemies, traps, pickups);
        }

        private SpawnConfig<T>[] GenerateSpawnConfigForType<T>(int total, List<T> pool, float roomSize, Vector2 roomPosition)
        {
            var configs = new SpawnConfig<T>[total];
            var usedPositions = new List<Vector3>();
            float minDistanceBetweenObjects = 2f; // Minimum distance to avoid overlap

            for (int index = 0; index < total; index++)
            {
                T type = pool[UnityEngine.Random.Range(0, pool.Count)];
                Vector3 position = this.GetNonOverlappingPosition(roomSize, roomPosition, usedPositions, minDistanceBetweenObjects);

                configs[index] = new SpawnConfig<T>
                {
                    Type = type,
                    Position = position
                };

                usedPositions.Add(position);
            }

            return configs;
        }

        private Vector3 GetNonOverlappingPosition(float roomSize, Vector2 roomPosition, List<Vector3> usedPositions, float minDistance)
        {
            Vector3 newPosition;
            bool isPositionValid;
            float margin = roomSize * 0.2f; // 20% margin from the sides
            float minX = (-roomSize / 2f) + margin;
            float maxX = (roomSize / 2f) - margin;
            float minZ = (-roomSize / 2f) + margin;
            float maxZ = (roomSize / 2f) - margin;

            do
            {
                // Randomize position within the adjusted bounds, considering the room position
                newPosition = new Vector3(
                    UnityEngine.Random.Range(minX, maxX) + roomPosition.x,
                    0,
                    UnityEngine.Random.Range(minZ, maxZ) + roomPosition.y
                );

                // Check if this position is far enough from all previously used positions
                isPositionValid = true;
                foreach (Vector3 usedPosition in usedPositions)
                {
                    if (Vector3.Distance(newPosition, usedPosition) < minDistance)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
            } while (!isPositionValid);

            return newPosition;
        }
    }
}