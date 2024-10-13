using Game.Database;
using UnityEngine;

namespace Game
{
    public class EnemyHealthController : HealthController
    {
        [SerializeField] private SpawnTypeEnemy _type; 

        private void Awake()
        {
            base.HealthRecoverListener += this.OnHealthRecover;
            base.HealthLoseListener += this.OnHealthLose;
        }

        private void OnValidate()
        {
            EnemyConfigDatabase database = Resources.Load<EnemyConfigDatabase>(ProjectPaths.ENEMY_CONFIG_DATABASE);
            EnemyConfig config = database.Config[(int)this._type];
            base._maxHealth = config.MaxHealth;
            base._deathVFX = config.DeathVFX;
            base._deathSFX = config.DeathSFX;
        }

        private void OnHealthRecover(float amount, float currentHealth, float maxHealth)
        {

        }

        private void OnHealthLose(float amount, float currentHealth, float maxHealth)
        {

        }
    }
}
