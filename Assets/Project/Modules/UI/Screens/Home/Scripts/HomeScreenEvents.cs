using ScreenNavigation;
using UnityEngine;
using Utils;

namespace Game
{
    public class HomeScreenEvents : MonoBehaviour
    {
        #region Inspector Events
        public void OnStartButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.CHARACTER_SETUP);
        }

        public void OnSettingsClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.SETTINGS);
        }
        #endregion
    }
}