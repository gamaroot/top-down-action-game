using UnityEngine;

namespace Utils
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private Tags _targetTag = Tags.Player;

        [field: SerializeField, ReadOnly] public GameObject Target { get; private set; }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(this._targetTag.ToString()))
            {
                this.Target = collider.gameObject;
                Debug.Log($"Sensor: \"{base.name}\" detected \"{collider.name}\"");
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.CompareTag(this._targetTag.ToString()))
            {
                this.Target = null;
                Debug.Log($"Sensor: \"{base.name}\" lost \"{collider.name}\"");
            }
        }
    }
}
