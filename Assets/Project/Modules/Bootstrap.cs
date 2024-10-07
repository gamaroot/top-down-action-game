using DG.Tweening;
using ScreenNavigation;
using Unity.Cinemachine;
using UnityEngine;
using Utils;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneID _firstScene;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            DOTween.Init(this);
#if UNITY_WEBGL
            Application.targetFrameRate = -1;
#else
            Application.targetFrameRate = GamePreferences.FPS;
#endif
            CameraHandler.Load(this._mainCamera, this._cinemachineBrain);
        }

        private void Start()
        {
            SceneNavigator.Initialize();
            SceneNavigator.Instance.LoadAdditiveSceneAsync(this._firstScene);

            Destroy(base.gameObject);
        }
    }
}