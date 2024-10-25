using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(AIMovementController), typeof(EnemyHealthController))]
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] protected AIMovementController _movementController;
        [SerializeField, ReadOnly] protected WeaponController _weaponController;
        [SerializeField, ReadOnly] protected EnemyHealthController _healthController;

        protected virtual void OnValidate()
        {
            if (this._movementController == null)
                this._movementController = this.GetComponent<AIMovementController>();

            if (this._weaponController == null)
                this._weaponController = this.GetComponentInChildren<WeaponController>();

            if (this._healthController == null)
                this._healthController = this.GetComponentInChildren<EnemyHealthController>();
        }

        public void Init(IGameManager gameManager)
        {
            IEnemyConfig enemyConfig = gameManager.EnemyConfig[(int)this.Type];
            this._healthController.Init(enemyConfig);
            this._healthController.Listener.OnDeath = () => gameManager.OnEnemyKill(enemyConfig);

            if (this._weaponController != null)
                this._weaponController.Init(gameManager.WeaponConfig);
        }

        public void FaceTarget(Vector3 point)
        {
            this._movementController.RotateY(base.transform.position, point);
        }

        public virtual void OnMove(Vector3 point)
        {
            this._movementController.Move(point);
        }

        protected abstract SpawnTypeEnemy Type { get; }
    }
}