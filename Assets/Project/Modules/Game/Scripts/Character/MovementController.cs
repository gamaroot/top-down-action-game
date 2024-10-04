using UnityEngine;

namespace Game
{
    public abstract class MovementController : MonoBehaviour
    {
        public abstract void Move(Vector3 point);
    }
}