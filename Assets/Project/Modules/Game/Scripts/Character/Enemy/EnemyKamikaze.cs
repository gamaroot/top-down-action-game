using UnityEngine;
using Utils;

namespace Game
{
    public class EnemyKamikaze : Enemy
    {
        [SerializeField] private EnemyHealthController _healthController;

        public void OnCloseToTarget()
        {
            this._healthController.OnDeath();
        }
    }
}
