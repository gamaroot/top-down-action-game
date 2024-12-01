namespace Game
{
    public class EnemyKamikaze : Enemy
    {
        public void OnCloseToTarget()
        {
            base._healthController.OnDeath(true);
        }
    }
}
