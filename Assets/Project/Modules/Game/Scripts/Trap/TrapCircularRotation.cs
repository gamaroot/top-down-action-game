using UnityEngine;

namespace Game
{
    public class TrapCircularRotation : MonoBehaviour
    {
        [SerializeField] float _rotationSpeed;

        void Update()
        {
            base.gameObject.transform.Rotate(0, 0, this._rotationSpeed * Time.deltaTime);
        }
    }
}
