using DG.Tweening;
using ScreenNavigation;
using UnityEngine;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneID _firstScene;

        private void Awake()
        {
            DOTween.Init(this);
#if UNITY_WEBGL
            Application.targetFrameRate = -1;
#else
            Application.targetFrameRate = GamePreferences.FPS;
#endif
        }

        private void Start()
        {
            SceneNavigator.Initialize();
            SceneNavigator.Instance.LoadAdditiveSceneAsync(this._firstScene);

            Destroy(base.gameObject);
        }
    }
}