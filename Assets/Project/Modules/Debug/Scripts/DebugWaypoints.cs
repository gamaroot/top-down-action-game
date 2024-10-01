using UnityEngine;
using Utils;

namespace Game
{
    public class DebugWaypoints : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, ReadOnly] private Transform[] _waypoints;

        private void OnValidate()
        {
            if (this._waypoints == null || this._waypoints.Length == 0)
                this._waypoints = base.GetComponentsInChildren<Transform>();
        }

        private void Update()
        {
            if (this._waypoints == null || this._waypoints.Length < 2)
                return;

            // Ignoring first element since it's the parent object
            for (int index = 1; index < this._waypoints.Length; index++)
            {
                int nextIndex = index == this._waypoints.Length - 1 ? 1 : index + 1;
                
                Vector3 currentPoint = this._waypoints[index].position;
                Vector3 nextPoint = this._waypoints[nextIndex].position;

                Debug.DrawLine(currentPoint, nextPoint, Color.yellow);
            }
        }
#endif
    }
}