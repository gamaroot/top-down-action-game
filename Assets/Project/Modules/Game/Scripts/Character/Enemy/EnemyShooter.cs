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

        public void OnAttack(Vector3 point)
        {
            if (base._weapon.CanShoot)
                base._weapon.ShootAtTarget(point);
        }
    }
}