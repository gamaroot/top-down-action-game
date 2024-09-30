using ScreenNavigation;
using UnityEngine;

namespace Game
{
    public class DebugScreenEvents : MonoBehaviour
    {
        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.DEBUG, SceneID.HOME);
        }
    }
}