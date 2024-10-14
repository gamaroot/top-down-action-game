using Game.Database;

namespace Game
{
    public class PlayerParryController : ParryController
    {
        private ICharacterConfig _config;

        public void Init(ICharacterConfig config)
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
