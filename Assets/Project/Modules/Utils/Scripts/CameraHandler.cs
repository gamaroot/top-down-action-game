using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Utils
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraHandler : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float _strength;
        [SerializeField] private float _duration;

        [Header("Components")]
        [SerializeField] private CinemachineVirtualCamera _vcam;

        public Camera MainCamera { get; private set; }

        private CinemachineBasicMultiChannelPerlin _perlinNoise;

        private void OnValidate()
        {
            this._vcam = base.GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            this.MainCamera = Camera.main;
            this._perlinNoise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            this._perlinNoise.m_AmplitudeGain = this._strength;
            base.StartCoroutine(this.StopShake(this._duration));
        }

        private IEnumerator StopShake(float delay)
        {
            yield return new WaitForSeconds(delay);
            this._perlinNoise.m_AmplitudeGain = 0;
        }
    }
}