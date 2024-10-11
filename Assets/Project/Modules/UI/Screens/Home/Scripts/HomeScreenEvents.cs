using ScreenNavigation;
using UnityEngine;

namespace Game
{
    public class HomeScreenEvents : MonoBehaviour
    {
        #region Inspector Events
        public void OnStartButtonClick()
        {
            //(SceneID, bool) sceneParams = (this._gameStateHandler.GameState.Stage, this._gameStateHandler.HasSavedGame);
            //SceneNavigator.Instance.SetSceneParams(SceneID.LOADING, sceneParams);
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.DEBUG);
        }

        public void OnSettingsClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.SETTINGS);
        }
        #endregion
    }
}