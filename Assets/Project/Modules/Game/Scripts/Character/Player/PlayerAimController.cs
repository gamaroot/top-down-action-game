using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Game
{
    public class PlayerAimController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private LayerMask _groundLayerMask;

        private void Update()
        {
            bool isGamepad = Gamepad.current != null;
            if (isGamepad)
                this.RotateToGamepadDirection();
            else
                this.RotateToMousePoint();
        }

        private void RotateToMousePoint()
        {
            Vector2 mousePosition = Mouse.current.position.value;
            Ray ray = CameraHandler.Instance.MainCamera.ScreenPointToRay(mousePosition);

            // Check if the ray hits the ground
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, this._groundLayerMask))
            {
                // Get the direction from the player to the hit point
                Vector3 direction = hit.point - base.transform.position;
                this.RotateToDirection(new Vector2(direction.x, direction.z));
            }
        }

        private void RotateToGamepadDirection()
        {
            Vector2 direction = Gamepad.current.rightStick.value;
            this.RotateToDirection(direction);
        }

        private void RotateToDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            // Ensure the direction vector is normalized and ignore the y-axis
            var flatDirection = new Vector3(direction.x, 0, direction.y);
            base.transform.rotation = Quaternion.LookRotation(flatDirection);
        }
    }
}