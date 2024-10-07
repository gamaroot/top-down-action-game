using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer), typeof(AIMovementController))]
    public class EnemyShooter : Enemy
    {
        public override void OnMove(Vector3 point)
        {
            this._movement.SetStoppingDistance(this._weapon.Range);
            base.OnMove(point);
        }

        public void OnTargetLost()
        {
            this._movement.SetStoppingDistance(0);
        }

        public void OnAttack()
        {
            if (base._weapon.CanShoot)
                base._weapon.Shoot();
        }
    }
}