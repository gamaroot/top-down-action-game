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

        private Bloom _bloomLayer;
        private Vignette _vignetteLayer;
        private ChromaticAberration _chromaticAberrationLayer;

        private ScriptableRendererFeature _fullScreenFX;

        private void Awake()
        {
            this._fullScreenFX = this._urpAsset.rendererFeatures[1];
            this._postProcess.profile.TryGet<Bloom>(out this._bloomLayer);
            this._postProcess.profile.TryGet<Vignette>(out this._vignetteLayer);
            this._postProcess.profile.TryGet<ChromaticAberration>(out this._chromaticAberrationLayer);
        }

        public void ShowDamageLayer(float healthPercentage)
        {
            this._vignetteLayer.color.value = this._damageLayerColor;
            this.AnimateVignetteLayer(this._damageLayerIntensity, this._damageLayerDuration, this._damageLayerEase);

            this.UpdateCriticalDamageLayer(healthPercentage);
        }

        public void ShowHealLayer(float healthPercentage)
        {
            this._vignetteLayer.color.value = this._healLayerColor;
            this.AnimateVignetteLayer(this._healLayerIntensity, this._healLayerDuration, this._healLayerEase);

            this.UpdateCriticalDamageLayer(healthPercentage);
        }

        public void DisableAll()
        {
            this._tweenVignette?.Kill();
            this._fullScreenFX.SetActive(false);

            this._bloomLayer.tint.value = Color.white;

            this._vignetteLayer.intensity.value = 0;
            this._vignetteLayer.active = false;

            this._chromaticAberrationLayer.intensity.value = 0;
            this._chromaticAberrationLayer.active = false;
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
                        this._vignetteLayer.intensity.value = currentValue;
                    })
                    .OnComplete(() =>
                    {
                        this._vignetteLayer.active = false;
                    });
        }

        private void UpdateCriticalDamageLayer(float healthPercentage)
        {
            float intensity = 1f - healthPercentage;

            this._bloomLayer.tint.value = Color.Lerp(Color.white, Color.red, intensity);

            this._chromaticAberrationLayer.active = intensity != 0;
            this._chromaticAberrationLayer.intensity.value = intensity;

            this._fullScreenFX.SetActive(intensity > 0 && intensity > 0.7f);
            this._fullScreenFXMaterial.SetFloat("_VignetteIntensity", Mathf.Clamp(intensity, 0.8f, 1f));
        }
    }
}