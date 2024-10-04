using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer), typeof(AIMovementController))]
    public class Enemy : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, ReadOnly] public MeshRenderer _meshRenderer;
        [SerializeField, ReadOnly] public AIMovementController _movement;
        [SerializeField, ReadOnly] public WeaponController _weapon;

        protected virtual void OnValidate()
        {
            if (this._meshRenderer == null)
                this._meshRenderer = this.GetComponent<MeshRenderer>();

            if (this._movement == null)
                this._movement = this.GetComponent<AIMovementController>();

            if (this._weapon == null)
                this._weapon = this.GetComponentInChildren<WeaponController>();
        }

        public void FaceTarget(Vector3 point)
        {
            this._movement.RotateY(base.transform.position, point);
        }

        public virtual void OnMove(Vector3 point)
        {
            this._movement.Move(point);
        }
    }
}