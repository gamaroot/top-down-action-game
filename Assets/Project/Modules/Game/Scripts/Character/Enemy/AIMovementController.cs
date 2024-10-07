using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovementController : MovementController
    {
        [SerializeField] private GameObject _aim;
        [SerializeField, ReadOnly] private NavMeshAgent _agent;

        private void OnValidate()
        {
            if (this._agent == null)
                this._agent = this.GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            this._aim.SetActive(true);
        }

        private void OnDisable()
        {
            this._aim.SetActive(false);
        }

        public void SetStoppingDistance(float stoppingDistance)
        {
            this._agent.stoppingDistance = stoppingDistance;
        }

        public override void Move(Vector3 point)
        {
            this._agent.SetDestination(point);
        }

        public void RotateY(Vector3 myPosition, Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - myPosition).normalized;
            var rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;
            base.transform.rotation = rotation;
        }
    }
}