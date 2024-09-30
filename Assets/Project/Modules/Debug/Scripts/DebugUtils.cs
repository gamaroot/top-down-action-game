using UnityEngine;

namespace Game
{
    public class DebugUtils
    {
        public Vector3 GetRandomCircularPosition(Vector3 center, float distanceFromCenter, float range)
        {
            // Generate a random angle in radians
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);

            // Calculate a random distance within the specified range
            float randomDistance = distanceFromCenter + Random.Range(-range / 2f, range / 2f);

            // Calculate the random position using polar coordinates
            var randomPosition = new Vector3(
                center.x + (randomDistance * Mathf.Cos(randomAngle)),
                center.y,
                center.z + (randomDistance * Mathf.Sin(randomAngle))
            );
            return randomPosition;
        }
    }
}