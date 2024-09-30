using Game.Database;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private LayerMask _weaponLayerMask;

        [Header("Components")]
        [SerializeField, ReadOnly] public Sensor _sensor;
        [SerializeField, ReadOnly] public NavMeshAgent _agent;
        [SerializeField] private Transform _shootPoint;

        private float _lastTimeShot;
        private int _weaponLayerIndex;

        private void OnValidate()
        {
            if (this._sensor == null)
                this._sensor = this.GetComponent<Sensor>();

            if (this._agent == null)
                this._agent = this.GetComponent<NavMeshAgent>();
        }

        private void Awake()
        {
            this._weaponLayerIndex = Mathf.RoundToInt(Mathf.Log(_weaponLayerMask.value, 2));
        }

        private void Update()
        {
            this._lastTimeShot += Time.deltaTime;
        }

        public void OnTargetOnSight(Transform target, float keepDistanceFromTarget, bool isKamikaze)
        {
            if (isKamikaze) // Just chase the target
            {
                this.MoveToDestination(target.position);
                return;
            }

            if (this._sensor.IsTargetBehindObstacle)
            {
                // Reposition to shoot the target
                this.MoveToDestination(this._sensor.BestShootingPosition);
            }
            else if (this._lastTimeShot > this._weaponConfig.FireRate)
            {
                this.ChaseTarget(target, keepDistanceFromTarget);
                this.ShootTarget(target);
            }
        }

        private void ChaseTarget(Transform target, float keepDistanceFromTarget)
        {
            Vector3 destination = target.position;
            Vector3 directionToTarget = (destination - base.transform.position).normalized;

            // Calculate the new position, keeping a distance from the target
            destination -= directionToTarget * keepDistanceFromTarget;

            this.MoveToDestination(destination);
        }

        private void ShootTarget(Transform target)
        {
            // Aim at the target
            Vector3 directionToTarget = (target.position - this._shootPoint.position).normalized;
            this._shootPoint.rotation = Quaternion.LookRotation(directionToTarget);

            Bullet bullet = SpawnablePool.Spawn<Bullet>(SpawnType.BULLET_0);
            bullet.gameObject.layer = this._weaponLayerIndex;
            bullet.SetWeaponConfig(this._weaponConfig);
            bullet.Shoot(this._shootPoint);

            this._lastTimeShot = 0f;
        }

        private void MoveToDestination(Vector3 position)
        {
            this._agent.SetDestination(position);
        }
    }
}