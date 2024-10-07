namespace Game
{
    public class EnemyHealthController : HealthController
    {
        private void Awake()
        {
            base.HealthRecoverListener += this.OnHealthRecover;
            base.HealthLoseListener += this.OnHealthLose;
        }

        private void OnHealthRecover(float amount, float currentHealth, float maxHealth)
        {

        }

        private void OnHealthLose(float amount, float currentHealth, float maxHealth)
        {

        }
    }
}
