using Game.Database;
using UnityEngine;

namespace Game
{
    public class EnemyHealthController : HealthController
    {
        private bool _isKamikaze;

        public void Init(ICharacterConfig config, bool isKamikaze)
        {
            base.Init(config);
            this._isKamikaze = isKamikaze;
        }

        protected override void OnSpawnDeathFX()
        {
            if (!this._isKamikaze)
            {
                base.OnSpawnDeathFX();
                return;
            }

            SFX.PlayKamikaze();
            GameObject vfx = SpawnablePool.SpawnKamikaze();
            vfx.transform.position = base.transform.position;
            vfx.gameObject.SetActive(true);
        }
    }
}
