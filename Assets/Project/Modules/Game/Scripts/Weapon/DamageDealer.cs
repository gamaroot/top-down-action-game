using Game.Database;
using UnityEngine;

namespace Game
{
    public class DamageDealer : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; private set; } = 1f;

        public float ID { get; private set; }

        private void OnEnable()
        {
            this.ID = Random.value;
        }

        public void LoadConfig(WeaponConfig config)
        {
            this.Damage = config.Damage;
        }
    }
}