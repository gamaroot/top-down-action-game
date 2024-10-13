using UnityEngine;

namespace Game
{
    public class DebugUtils
    {
        public Vector3 GetRandomCircularPosition(Vector3 center, float distanceFromCenter, float range, float objectRadius, LayerMask collisionLayer)
        {
            const int maxAttempts = 100;
            int attempts = 0;

            Vector3 randomPosition = Vector3.zero;
            bool positionIsValid = false;

            while (!positionIsValid && attempts < maxAttempts)
            {
                // Generate a random angle in radians
                float randomAngle = Random.Range(0f, Mathf.PI * 2f);

                // Calculate a random distance within the specified range
                float randomDistance = distanceFromCenter + Random.Range(-range / 2f, range / 2f);

                // Calculate the random position using polar coordinates
                randomPosition = new Vector3(
                    center.x + (randomDistance * Mathf.Cos(randomAngle)),
                    center.y,
                    center.z + (randomDistance * Mathf.Sin(randomAngle))
                );

                // Check if there are any overlapping objects at the new position
                Collider[] hitColliders = Physics.OverlapSphere(randomPosition, objectRadius, collisionLayer);

                // If no overlapping colliders, the position is valid
                if (hitColliders.Length == 0)
                {
                    positionIsValid = true;
                }

                attempts++;
            }

            // If no valid position is found after max attempts, return the original center position
            return positionIsValid ? randomPosition : center;
        }

    }
}