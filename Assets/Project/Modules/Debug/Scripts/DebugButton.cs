using ScreenNavigation;
using UnityEngine;

namespace Game
{
    public class DebugButton : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            base.gameObject.SetActive(true);
#else
            Destroy(base.gameObject);
#endif
        }

#if UNITY_EDITOR
        public void OnDebugButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.HOME, SceneID.DEBUG);
        }
#endif
    }
}