using UnityEngine;

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
        }

        private void OnMapTriggered()
        {
            bool isMapVisible = !this.IsMapVisible;
            if (isMapVisible)
            {
                this._mapAnimator.gameObject.SetActive(true);
            }
            this._mapAnimator.SetBool(AnimationKeys.VISIBLE, isMapVisible);
        }
    }
}