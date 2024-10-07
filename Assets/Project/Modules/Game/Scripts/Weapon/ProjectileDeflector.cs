using UnityEngine;

namespace Game
{
    public class ProjectileDeflector : MonoBehaviour
    {
        [SerializeField] private LayerMask _originalProjectileLayerMask;
        [SerializeField] private LayerMask _convertedProjectileLayerMask;
        [SerializeField] private float _deflectionSpeedMultiplier = 1f;

        private int _originalProjectileLayerIndex;
        private int _convertedProjectileLayerIndex;

        private void Awake()
        {
            this._originalProjectileLayerIndex = (int)Mathf.Log(this._originalProjectileLayerMask.value, 2);
            this._convertedProjectileLayerIndex = (int)Mathf.Log(this._convertedProjectileLayerMask.value, 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == this._originalProjectileLayerIndex)
            {
                // Deflect the projectile back and turns it into your own projectile
                other.gameObject.layer = this._convertedProjectileLayerIndex;

                Vector3 deflectionDirection = (other.transform.position - base.transform.position).normalized;
                deflectionDirection.y = base.transform.position.y;

                // Reverse or change the direction
                float currentSpeed = other.attachedRigidbody.linearVelocity.magnitude;
                Vector3 velocity = currentSpeed * this._deflectionSpeedMultiplier * deflectionDirection;
                velocity.y = base.transform.position.y;

                other.attachedRigidbody.linearVelocity = velocity;

                // Rotates the projectile to face the new direction
                other.transform.rotation = Quaternion.LookRotation(deflectionDirection);
            }
        }
    }
}
