using Game.Database;
using Utils;

namespace Game
{
    public class PlayerHealthController : HealthController
    {
        public override int MaxHealth => this._overridenStats.MaxHealth;

        private CharacterStats _overridenStats;

        private void OnDisable()
        {
            CameraHandler.Instance.StopShake();
        }

        public void Init(ICharacterConfig config, CharacterStats overridenStats, CharacterHealthEvents listener)
        {
            this._overridenStats = overridenStats;
            base.Init(config);

            this.Listener = listener;
        }

        public void ApplyDamage(int amount)
        {
            base.TakeDamage(amount);
        }
    }
}