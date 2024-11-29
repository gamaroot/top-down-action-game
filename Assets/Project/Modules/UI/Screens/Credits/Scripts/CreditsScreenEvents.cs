using ScreenNavigation;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CreditsScreenEvents : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _scrollSpeed = 0.1f;

        private void OnValidate()
        {
            if (this._scrollRect == null)
                this._scrollRect = base.GetComponentInChildren<ScrollRect>();
        }

        private void Awake()
        {
            this._scrollRect.verticalNormalizedPosition = 1f;
        }

        private void Update()
        {
            this._scrollRect.verticalNormalizedPosition -= this._scrollSpeed * Time.deltaTime;
            if (_scrollRect.verticalNormalizedPosition < 0)
                this.GoToHomeScreen();
        }

        // Called in the inspector
        public void OnSkipButtonClick()
        {
            this.GoToHomeScreen();
        }

        private void GoToHomeScreen()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.CREDITS, SceneID.HOME);
        }
    }
}
