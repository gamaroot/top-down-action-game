using UnityEngine;

namespace Game
{
    public class DamageDealer : MonoBehaviour
    {
        public float ID { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

        private void OnEnable()
        {
            this.ID = Random.value;
        }

        public void SetDamage(int damage)
        {
            this.Damage = damage;
        }
    }
}