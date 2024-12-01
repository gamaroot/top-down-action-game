using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private Tags _targetTag = Tags.Player;
        [SerializeField] private Tags _enemyBulletTag = Tags.PlayerBullet;
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField, ReadOnly] private SphereCollider _sphereCollider;

        public Transform IncomingBullet { get; private set; }

        public GameObject Target { get; private set; }
        public bool IsTargetOnSight => this.Target != null && this.Target.activeSelf;
        public bool IsTargetBehindObstacle { get; private set; }
        
        public Vector3 BestShootingPosition { get; private set; }

        private void OnValidate()
        {
            if (this._sphereCollider == null)
                this._sphereCollider = this.GetComponent<SphereCollider>();
        }

        private void Update()
        {
            if (this.Target != null)
            {
                this.CheckIfTargetIsBehindObstacle();
            }
            else
            {
                this.IsTargetBehindObstacle = false;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(this._targetTag.ToString()))
            {
                this.Target = collider.gameObject;
                Debug.Log($"Sensor: \"{base.name}\" detected \"{collider.name}\"");
            }
            else if (collider.CompareTag(this._enemyBulletTag.ToString()))
            {
                // Checks if collider is going to the direction of the sensor
                Vector3 direction = (collider.transform.position - base.transform.position).normalized;
                float dot = Vector3.Dot(direction, base.transform.forward);

                // If the dot product is positive, the bullet is coming towards the sensor
                if (dot > 0)
                {
                    if (this.IncomingBullet != null)
                    {
                        // If there's already an incoming bullet, compare the distance
                        float distanceToNewBullet = Vector3.Distance(base.transform.position, collider.transform.position);
                        float distanceToOldBullet = Vector3.Distance(base.transform.position, this.IncomingBullet.position);
                        if (distanceToNewBullet > distanceToOldBullet)
                            return;
                    }
                    this.IncomingBullet = collider.transform;
                    Debug.Log($"Sensor: \"{base.name}\" detected an incoming bullet \"{collider.name}\"");
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag(this._targetTag.ToString()))
            {
                this.Target = null;
                Debug.Log($"Sensor: \"{base.name}\" lost \"{collider.name}\"");
            }
        }

        private void CheckIfTargetIsBehindObstacle()
        {
            Vector3 sensorPosition = base.transform.position;
            Vector3 targetPosition = this.Target.transform.position;

            // Direction and distance between sensor and target
            Vector3 direction = (targetPosition - sensorPosition).normalized;
            float distance = Vector3.Distance(sensorPosition, targetPosition);

            // Check if there's an obstacle between the sensor and the target
            if (Physics.Raycast(sensorPosition, direction, out RaycastHit hit, distance, this._obstacleLayer))
            {
                this.IsTargetBehindObstacle = true;
                this.BestShootingPosition = this.GetBestShootingPosition(this._sphereCollider.radius);
            }
            else
            {
                this.IsTargetBehindObstacle = false;
                Debug.DrawLine(sensorPosition, targetPosition, Color.green);
            }
        }

        private Vector3 GetBestShootingPosition(float searchRadius, int searchAngleSteps = 30)
        {
            Vector3 bestPosition = Vector3.zero;
            Vector3 sensorPosition = base.transform.position;
            Vector3 targetPosition = this.Target.transform.position;

            float bestDistance = Mathf.Infinity;

            // Sweep around the sensor in a circle to find the best clear path
            for (int index = 0; index < 360; index += searchAngleSteps)
            {
                float angleInRad = index * Mathf.Deg2Rad;

                // Trigonometry: Coords of a point in a circle is x = cos(angle), z = sin(angle)
                var direction = new Vector3(Mathf.Cos(angleInRad), 0, Mathf.Sin(angleInRad));

                Vector3 candidatePosition = sensorPosition + (direction * searchRadius);
                Vector3 directionToTarget = (targetPosition - candidatePosition).normalized;
                float distanceToTarget = Vector3.Distance(candidatePosition, targetPosition);

                // Check if the line to the target is unobstructed
                if (!Physics.Raycast(candidatePosition, directionToTarget, distanceToTarget, this._obstacleLayer))
                {
                    // If this position is closer than the best so far, use it
                    if (distanceToTarget < bestDistance)
                    {
                        bestDistance = distanceToTarget;
                        bestPosition = candidatePosition;
                    }

                    // Visualize candidate positions
                    Debug.DrawLine(candidatePosition, targetPosition, Color.blue);
                }
                else
                {
                    Debug.DrawLine(candidatePosition, targetPosition, Color.red);
                }
            }

            return bestPosition;
        }
    }
}
