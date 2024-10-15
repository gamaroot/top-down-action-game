using UnityEngine;

namespace Game
{
    public class TrapDamageController : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 1f; 
        [SerializeField] private float damageInterval = 1f;  
        private float lastDamageTime; 
        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealthController playerHealth) && !hasTriggered)
            {
                playerHealth.ApplyDamage(damageAmount);
                lastDamageTime = Time.time;
                hasTriggered = true;  
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealthController playerHealth))
            {
                if (Time.time >= lastDamageTime + damageInterval)
                {
                    playerHealth.ApplyDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealthController playerHealth))
            {
                hasTriggered = false;
            }
        }
    }
}
