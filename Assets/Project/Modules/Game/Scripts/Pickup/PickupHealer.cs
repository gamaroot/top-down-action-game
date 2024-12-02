using UnityEngine;

namespace Game
{
    public class PickupHealer : MonoBehaviour
    {
        [SerializeField] private SpawnTypePickup _type;
        [field: SerializeField] public int Heal { get; private set; }

        private void OnTriggerEnter(Collider _)
        {
            SFX.PlayPickup(this._type);
            this.gameObject.SetActive(false);
        }
    }
}