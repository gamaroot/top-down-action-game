using Unity.Cinemachine;
using UnityEngine;
using Utils;

namespace Game
{
    public class MapScreenEvents : MonoBehaviour
    {
        [SerializeField] private Animator _mapAnimator;

        private InputController _input;

        private void Awake()
        {
            this._input = new InputController();
            this._input.Player.Map.performed += _ => this.OnMapTriggered();
        }

        private void OnEnable()
        {
            this._input.Enable();
        }

        private void OnDisable()
        {
            this._input.Disable();
            this.SwitchCamera(isMapVisible: false);
        }

        private void OnMapTriggered()
        {
            if (this.CanTriggerMap())
                return;

            bool isMapVisible = !this._mapAnimator.GetBool(AnimationKeys.VISIBLE);
            this._mapAnimator.SetBool(AnimationKeys.VISIBLE, isMapVisible);

            this.SwitchCamera(isMapVisible);
        }

        private bool CanTriggerMap()
        {
            // Ignore the input when an animation is transitioning
            if (this._mapAnimator.IsInTransition(0))
                return true;

            // Ignore the input when an animation didn't complete 75% of its duration
            float animationTime = this._mapAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return animationTime > 0 && animationTime < 0.75f;
        }

        private void SwitchCamera(bool isMapVisible)
        {
            CinemachineCamera playerCamera = CameraHandler.Instance.PlayerCamera;
            playerCamera.Priority = isMapVisible ? 0 : 1;

            CinemachineCamera mapCamera = CameraHandler.Instance.MapCamera;
            mapCamera.Priority = isMapVisible ? 1 : 0;
        }
    }
}