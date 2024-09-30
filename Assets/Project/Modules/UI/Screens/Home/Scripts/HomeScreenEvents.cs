using ScreenNavigation;
using UnityEngine;

namespace Game
{
    public class HomeScreenEvents : MonoBehaviour
    {
        //private GameStateHandler _gameStateHandler;

        private void Awake()
        {
            //this._gameStateHandler = new GameStateHandler();
        }

        #region Inspector Events
        public void OnStartButtonClick()
        {
            //(SceneID, bool) sceneParams = (this._gameStateHandler.GameState.Stage, this._gameStateHandler.HasSavedGame);
            //SceneNavigator.Instance.SetSceneParams(SceneID.LOADING, sceneParams);
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.LOADING);
        }

        public void OnSettingsClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.SETTINGS);
        }
        #endregion
    }
}