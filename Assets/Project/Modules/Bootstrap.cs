using DG.Tweening;
using ScreenNavigation;
using UnityEngine;
using Utils;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneID _firstScene;
        [SerializeField] private Camera _mainCamera;

        private void Awake()
        {
            DOTween.Init(this);
#if UNITY_WEBGL
            Application.targetFrameRate = -1;
#else
            Application.targetFrameRate = GamePreferences.FPS;
#endif
            CameraHandler.Load(this._mainCamera);
        }

        private void Start()
        {
            if (this._firstScene != SceneID.NONE)
            {
                SceneNavigator.Initialize();
                SceneNavigator.Instance.LoadAdditiveSceneAsync(this._firstScene);
            }

            Destroy(base.gameObject);
        }
    }
}