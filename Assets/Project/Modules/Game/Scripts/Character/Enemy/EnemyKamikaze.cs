using UnityEngine;

namespace Game
{
    public class EnemyKamikaze : Enemy
    {
        protected override SpawnTypeEnemy Type { get; } = SpawnTypeEnemy.KAMIKAZE;

        public void OnCloseToTarget()
        {
            base._healthController.OnDeath(true);
        }
    }
}
