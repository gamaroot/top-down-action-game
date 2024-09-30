using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class RendererEventsListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onBecameInvisible;

        private void OnBecameInvisible()
        {
            this._onBecameInvisible.Invoke();
        }
    }
}