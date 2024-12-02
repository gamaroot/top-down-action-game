using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(EventSystem), typeof(Selectable))]
    public class UINavigationSetup : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Selectable _selectable;

        private void OnValidate()
        {
            if (this._eventSystem == null)
                this._eventSystem = base.GetComponent<EventSystem>();

            if (this._selectable == null)
                this._selectable = base.GetComponent<Selectable>();

            this._eventSystem.firstSelectedGameObject = base.gameObject;
        }

        private void Awake()
        {
            bool isGamepadActive = Gamepad.current != null;
            if (isGamepadActive)
            {
                this._eventSystem.firstSelectedGameObject = this._selectable.navigation.selectOnDown.gameObject;
                this._selectable.navigation.selectOnDown.Select();
            }
        }
    }
}
