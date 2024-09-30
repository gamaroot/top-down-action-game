using UnityEngine;

namespace Utils
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private Tags _targetTag = Tags.Player;
        [SerializeField] private LayerMask _obstacleLayer;

        [field: SerializeField, ReadOnly] public GameObject Target { get; private set; }
        [field: SerializeField, ReadOnly] public bool IsTargetBehindObstacle { get; private set; }

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
                Debug.DrawLine(sensorPosition, hit.point, Color.red);
            }
            else
            {
                this.IsTargetBehindObstacle = false;
                Debug.DrawLine(sensorPosition, targetPosition, Color.green);
            }
        }
    }
}
