using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnerGenerator
    {
        public Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> GenerateSpawnConfig(RoomData config)
        {
            var enemies = new SpawnConfig<SpawnTypeEnemy>[config.TotalEnemies];
            var traps = new SpawnConfig<SpawnTypeTrap>[config.TotalTraps];

            var usedPositions = new List<Vector3>();
            float minDistanceBetweenObjects = 2f; // Minimum distance to avoid overlap

            // Spawn enemies
            for (int index = 0; index < config.TotalEnemies; index++)
            {
                SpawnTypeEnemy type = config.EnemyPool[UnityEngine.Random.Range(0, config.EnemyPool.Count)];
                Vector3 position = this.GetNonOverlappingPosition(config.SquaredSize, usedPositions, minDistanceBetweenObjects);

                enemies[index] = new SpawnConfig<SpawnTypeEnemy>
                {
                    Type = type,
                    Position = position
                };
                usedPositions.Add(position);
            }

            // Spawn traps
            for (int index = 0; index < config.TotalTraps; index++)
            {
                SpawnTypeTrap type = config.TrapPool[UnityEngine.Random.Range(0, config.TrapPool.Count)];
                Vector3 position = this.GetNonOverlappingPosition(config.SquaredSize, usedPositions, minDistanceBetweenObjects);

                traps[index] = new SpawnConfig<SpawnTypeTrap>
                {
                    Type = type,
                    Position = position
                };
                usedPositions.Add(position);
            }

            return new(enemies, traps);
        }

        private Vector3 GetNonOverlappingPosition(float roomSize, List<Vector3> usedPositions, float minDistance)
        {
            Vector3 newPosition;
            bool isPositionValid;
            do
            {
                // Random position within the room bounds
                newPosition = new Vector3(
                    UnityEngine.Random.Range(-roomSize / 2f, roomSize / 2f),
                    0,
                    UnityEngine.Random.Range(-roomSize / 2f, roomSize / 2f)
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
