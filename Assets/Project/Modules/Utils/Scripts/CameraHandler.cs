using UnityEngine;
using Unity.Cinemachine;

namespace Utils
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraHandler : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float _strength;
        [SerializeField] private float _duration;

        [Header("Components")]
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private CinemachineBasicMultiChannelPerlin _perlinNoise;

        public Camera MainCamera { get; private set; }

        private void OnValidate()
        {
            this._perlinNoise = base.GetComponent<CinemachineBasicMultiChannelPerlin>();
            this._perlinNoise = (CinemachineBasicMultiChannelPerlin)this._camera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
        }

        private void Start()
        {
            this.MainCamera = Camera.main;
        }

        private void OnDisable()
        {
            base.CancelInvoke();
            this._perlinNoise.AmplitudeGain = 0;
        }

        public void Shake()
        {
            this._perlinNoise.AmplitudeGain = this._strength;
            base.Invoke("StopShake", this._duration);
        }

        private void StopShake()
        {
            this._perlinNoise.AmplitudeGain = 0;
        }
    }
}