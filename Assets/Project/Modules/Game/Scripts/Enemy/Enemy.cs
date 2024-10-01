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
        [SerializeField] private bool _isKamikaze;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private LayerMask _weaponLayerMask;

        [Header("Components")]
        [SerializeField] private Transform _shootPoint;
        [SerializeField, ReadOnly] public Sensor _sensor;
        [SerializeField, ReadOnly] public NavMeshAgent _agent;
        [SerializeField, ReadOnly] private WeaponDatabase _weaponDatabase;

        private float _lastTimeShot;
        private int _weaponLayerIndex;
        private WeaponConfig _weaponConfig;

        private void OnValidate()
        {
            if (this._sensor == null)
                this._sensor = this.GetComponent<Sensor>();

            if (this._agent == null)
                this._agent = this.GetComponent<NavMeshAgent>();

            if (this._weaponDatabase == null)
                this._weaponDatabase = Resources.Load<WeaponDatabase>(ProjectPaths.WEAPON_DATABASE);
        }

        private void Awake()
        {
            this._weaponConfig = this._weaponDatabase.Weapons[(int)this._weaponType];
            this._weaponLayerIndex = Mathf.RoundToInt(Mathf.Log(_weaponLayerMask.value, 2));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tags.Player.ToString()) && this._isKamikaze)
            {
                this.PrepareForSelfDestruction();
            }
        }

        private void Update()
        {
            this._lastTimeShot += Time.deltaTime;
        }

        public void OnTargetOnSight(Transform target, float keepDistanceFromTarget)
        {
            if (this._isKamikaze) // Just chase the target
            {
                this.MoveToDestination(target.position);
                return;
            }

            if (this._sensor.IsTargetBehindObstacle)
            {
                // Reposition to shoot the target
                this.MoveToDestination(this._sensor.BestShootingPosition);
            }
            else if (this._lastTimeShot > this._weaponConfig.ShootInterval)
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

            Bullet bullet = SpawnablePool.SpawnProjectile<Bullet>(SpawnTypeProjectile.ENERGY_MISSILE);
            bullet.gameObject.layer = this._weaponLayerIndex;
            bullet.SetWeaponConfig(this._weaponConfig);
            bullet.Shoot(this._shootPoint);

            this._lastTimeShot = 0f;
        }

        private void MoveToDestination(Vector3 position)
        {
            this._agent.SetDestination(position);
        }

        private void PrepareForSelfDestruction()
        {
            this._agent.isStopped = true;
            this._agent.speed = 0f;

            ParticleSystem explosion = SpawnablePool.SpawnExplosion<ParticleSystem>(SpawnTypeExplosion.KAMIKAZE_EXPLOSION);
            explosion.transform.SetParent(base.transform.parent, false);
            explosion.gameObject.SetActive(true);

            base.Invoke(nameof(this.OnSelfDestroyed), explosion.main.duration);
        }

        private void OnSelfDestroyed()
        {
            base.gameObject.SetActive(false);
        }
    }
}