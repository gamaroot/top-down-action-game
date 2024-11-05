using Unity.Cinemachine;
using UnityEngine;
using Utils;

namespace Game
{
    public class MapScreenEvents : MonoBehaviour
    {
        [SerializeField] private Animator _mapAnimator;

        private bool IsMapVisible => this._mapAnimator.gameObject.activeSelf;

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
            bool isMapVisible = !this.IsMapVisible;
            this.SwitchCamera(isMapVisible);

            if (isMapVisible)
            {
                this._mapAnimator.gameObject.SetActive(true);
            }
            this._mapAnimator.SetBool(AnimationKeys.VISIBLE, isMapVisible);
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