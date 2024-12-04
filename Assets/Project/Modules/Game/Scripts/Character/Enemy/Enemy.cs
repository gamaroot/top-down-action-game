using Game.Database;
using UnityEngine;
using Utils;
using static UnityEngine.Rendering.STP;

namespace Game
{
    [RequireComponent(typeof(AIMovementController), typeof(EnemyHealthController))]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private SpawnTypeEnemy _type;

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

        private void OnEnable()
        {
            GameManager.OnEnemySpawnListener(this._type,
                                             this._healthController.MaxHealth,
                                             this._healthController.CurrentHealth);
        }

        public void Init(IGameManager gameManager)
        {
            IEnemyConfig config = gameManager.EnemyConfig[(int)this._type];
            this._healthController.Init(config, config.Type == SpawnTypeEnemy.KAMIKAZE);
            this._healthController.Listener.OnRecoverHealth = () =>
            {
                GameManager.OnEnemyHealthUpdateListener(config.Type,
                                                        this._healthController.MaxHealth,
                                                        this._healthController.CurrentHealth);
            };
            this._healthController.Listener.OnLoseHealth = () =>
            {
                GameManager.OnEnemyHealthUpdateListener(config.Type,
                                                        this._healthController.MaxHealth,
                                                        this._healthController.CurrentHealth);
            };
            this._healthController.Listener.OnDeath = () => gameManager.OnEnemyKill(config);


            this._weaponController?.Init(gameManager.WeaponConfig);
        }

        public void FaceTarget(Vector3 point)
        {
            this._movementController.RotateY(base.transform.position, point);
        }

        public virtual void OnMove(Vector3 point)
        {
            this._movementController.Move(point);
        }
    }
}