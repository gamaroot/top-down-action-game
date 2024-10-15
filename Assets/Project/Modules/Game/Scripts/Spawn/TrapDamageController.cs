using UnityEngine;

namespace Game
{
    public class TrapDamageController : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 10f; 
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.ApplyDamage(damageAmount);
            }
        }
    }
}
