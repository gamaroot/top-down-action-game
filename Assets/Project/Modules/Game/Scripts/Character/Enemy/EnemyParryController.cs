using Game.Database;

namespace Game
{
    public class EnemyParryController : ParryController
    {
        private IEnemyConfig _config;

        public void Init(IEnemyConfig config)
        {
            this._config = config;
        }

        public override float GetCooldown()
        {
            return this._config.ParryCooldown;
        }

        public override float GetDuration()
        {
            return this._config.ParryDuration;
        }
    }
}
