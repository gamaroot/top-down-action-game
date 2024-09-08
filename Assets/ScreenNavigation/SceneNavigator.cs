using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    public sealed class SceneNavigator : SceneLifecycleOperator
    {
        public static SceneNavigator Instance { private set; get; }

        private SceneNavigator()
        {
            SceneManager.sceneLoaded += base.OnSceneLoaded;
            SceneManager.sceneUnloaded += base.OnSceneUnloaded;
        }

        public static void Initialize()
        {
            Instance = new SceneNavigator();
        }

        public AsyncOperation LoadAdditiveSceneAsync(SceneID closingSceneID, SceneID openingSceneID, Action onComplete = null, bool clearOldSceneParams = false)
        {
            this.UnloadSceneAsync(closingSceneID);
            return this.LoadAdditiveSceneAsync(openingSceneID, onComplete, clearOldSceneParams);
        }

        public AsyncOperation LoadAdditiveSceneAsync(SceneID sceneID, bool clearOldSceneParams = false)
        {
            return base.StartShowScreenProcess(new SceneLoadData
            {
                ID = sceneID,
                Mode = LoadSceneMode.Additive
            }, clearOldSceneParams);
        }

        public AsyncOperation LoadAdditiveSceneAsync(SceneID sceneID, Action onComplete, bool clearOldSceneParams = false)
        {
            base.ExecuteOnShowAnimationComplete(sceneID, onComplete);

            return base.StartShowScreenProcess(new SceneLoadData
            {
                ID = sceneID,
                Mode = LoadSceneMode.Additive
            }, clearOldSceneParams);
        }

        public void UnloadSceneAsync(SceneID sceneID)
        {
            base.StartHideScreenProcess(sceneID);
        }

        public void UnloadSceneAsync(SceneID sceneID, Action onComplete)
        {
            base.ExecuteOnSceneUnload(sceneID, onComplete);
            base.StartHideScreenProcess(sceneID);
        }
    }
}
