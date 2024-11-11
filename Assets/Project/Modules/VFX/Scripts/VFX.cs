using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class VFX : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Volume _postProcess;
        [SerializeField] private UniversalRendererData _urpAsset;
        [SerializeField] private Material _fullScreenFXMaterial;

        [Header("Damage FX")]
        [SerializeField] private float _damageLayerIntensity;
        [SerializeField] private Color _damageLayerColor;
        [SerializeField] private float _damageLayerDuration;
        [SerializeField] private Ease _damageLayerEase;

        [Header("Heal FX")]
        [SerializeField] private float _healLayerIntensity;
        [SerializeField] private Color _healLayerColor;
        [SerializeField] private float _healLayerDuration;
        [SerializeField] private Ease _healLayerEase;

        private Tween _tweenVignette;
        private Vignette _vignetteLayer;
        private ChromaticAberration _chromaticAberrationLayer;

        private ScriptableRendererFeature _fullScreenFX;

        private void Awake()
        {
            this._fullScreenFX = this._urpAsset.rendererFeatures[1];
            this._postProcess.profile.TryGet<Vignette>(out this._vignetteLayer);
            this._postProcess.profile.TryGet<ChromaticAberration>(out this._chromaticAberrationLayer);
        }

        public void ShowDamageLayer(float healthPercentage)
        {
            this._vignetteLayer.color.value = this._damageLayerColor;
            this.AnimateVignetteLayer(this._damageLayerIntensity, this._damageLayerDuration, this._damageLayerEase);

            this.SetChromaticAberrationLayer(healthPercentage);
        }

        public void ShowHealLayer(float healthPercentage)
        {
            this._vignetteLayer.color.value = this._healLayerColor;
            this.AnimateVignetteLayer(this._healLayerIntensity, this._healLayerDuration, this._healLayerEase);

            this.SetChromaticAberrationLayer(healthPercentage);
        }

        public void DisableAll()
        {
            this._tweenVignette?.Kill();
            this._fullScreenFX.SetActive(false);
        }

        private void AnimateVignetteLayer(float intensity, float duration, Ease ease)
        {
            float currentValue = this._vignetteLayer.intensity.value;

            this._tweenVignette?.Kill();
            this._tweenVignette = DOTween.To(() => 0, x => currentValue = x, intensity, duration)
                    .SetEase(ease)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnStart(() =>
                    {
                        this._vignetteLayer.active = true;
                    })
                    .OnUpdate(() =>
                    {
                        Debug.LogWarning($"currentValue: {currentValue}");
                        this._vignetteLayer.intensity.value = currentValue;
                    })
                    .OnComplete(() =>
                    {
                        this._vignetteLayer.active = false;
                    });
        }

        private void SetChromaticAberrationLayer(float intensity)
        {
            this._fullScreenFX.SetActive(intensity > 0 && intensity <= 0.5f);
            this._fullScreenFXMaterial.SetFloat("_VignetteIntensity", 1f - intensity);

            this._chromaticAberrationLayer.active = intensity != 0;
            this._chromaticAberrationLayer.intensity.value = 1f - intensity;
        }
    }
}