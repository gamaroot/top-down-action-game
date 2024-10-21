using Game.Database;

namespace Game
{
    public class PlayerParryController : ParryController
    {
        private CharacterStats _stats;

        public void Init(CharacterStats stats)
        {
            this._stats = stats;
        }

        public override float GetCooldown()
        {
            return this._stats.ParryCooldown;
        }

        public override float GetDuration()
        {
            return this._stats.ParryDuration;
        }
    }
}
