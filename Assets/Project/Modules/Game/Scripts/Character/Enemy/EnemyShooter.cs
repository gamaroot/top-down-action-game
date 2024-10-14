using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer), typeof(AIMovementController))]
    public class EnemyShooter : Enemy
    {
        protected override SpawnTypeEnemy Type { get; } = SpawnTypeEnemy.SHOOTER_ENERGY_MISSILE;

        public override void OnMove(Vector3 point)
        {
            this._movementController.SetStoppingDistance(base._weaponController.Range);
            base.OnMove(point);
        }

        public void OnTargetLost()
        {
            this._movementController.SetStoppingDistance(0);
        }

        public void OnAttack()
        {
            if (base._weaponController.CanShoot)
                base._weaponController.Shoot();
        }
    }
}