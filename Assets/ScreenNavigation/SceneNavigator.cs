using System;
using UnityEditor.SearchService;
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
            if (SceneNavigator.Instance.IsSceneTransitioning)
                return null;

            this.UnloadSceneAsync(closingSceneID);

            base.ExecuteOnShowAnimationComplete(openingSceneID, onComplete);

            return base.StartShowScreenProcess(new SceneLoadData
            {
                ID = openingSceneID,
                Mode = LoadSceneMode.Additive
            }, clearOldSceneParams);
        }

        public AsyncOperation LoadAdditiveSceneAsync(SceneID sceneID, bool clearOldSceneParams = false)
        {
            if (SceneNavigator.Instance.IsSceneTransitioning)
                return null;

            return base.StartShowScreenProcess(new SceneLoadData
            {
                ID = sceneID,
                Mode = LoadSceneMode.Additive
            }, clearOldSceneParams);
        }

        public void UnloadSceneAsync(SceneID sceneID)
        {
            if (SceneNavigator.Instance.IsSceneTransitioning)
                return;

            base.StartHideScreenProcess(sceneID);
        }
    }
}
