using DG.Tweening;
using ScreenNavigation;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class SettingsScreenEvents : MonoBehaviour
    {
        [SerializeField] private Slider _sliderSoundVolume, _sliderMusicVolume;
        [SerializeField] private TextMeshProUGUI _textSoundVolume, _textMusicVolume, _textVersion;
        [SerializeField] private TMP_Dropdown _dropdownLanguage;
        [SerializeField] private RectTransform _fpsSelector;

        private Tween _tweenFPSSelector;
        private float _fpsSelectorAnchor;

        private InputController _input;

        private void Awake()
        {
            this._input = new InputController();
            this._input.UI.Close.performed += _ => this.OnCloseButtonClick();
        }

        private void Start()
        {
            this._sliderSoundVolume.value = GamePreferences.SoundVolume;
            this._sliderMusicVolume.value = GamePreferences.MusicVolume;

            this.OnUpdateSoundVolume(this._sliderSoundVolume.value);
            this.OnUpdateMusicVolume(this._sliderMusicVolume.value);

            this._sliderSoundVolume.onValueChanged.AddListener(this.OnUpdateSoundVolume);
            this._sliderMusicVolume.onValueChanged.AddListener(this.OnUpdateMusicVolume);

            this._dropdownLanguage.value = GamePreferences.LanguageIndex;
            this._dropdownLanguage.onValueChanged.AddListener(_ => this.OnUpdateLanguage());

            this.UpdateFPSSelector(GamePreferences.FPS switch
            {
                60 => 1f,
                120 => 2f,
                _ => 0
            }, GamePreferences.FPS);

            this._textVersion.text = Application.version.ToString();
        }

        private void OnEnable()
        {
            this._input.Enable();
        }

        private void OnDisable()
        {
            this._input.Disable();
        }

        public void OnCloseButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.SETTINGS, SceneID.HOME);
        }

        private void OnUpdateSoundVolume(float volume)
        {
            this._textSoundVolume.text = $"{volume * 100f:N0}%";
            GamePreferences.SoundVolume = volume;
        }

        private void OnUpdateMusicVolume(float volume)
        {
            this._textMusicVolume.text = $"{volume * 100f:N0}%";
            GamePreferences.MusicVolume = volume;
        }

        public void On30FPSButtonClick()
        {
            this.UpdateFPSSelector(0, 30);
        }

        public void On60FPSButtonClick()
        {
            this.UpdateFPSSelector(1f, 60);
        }

        public void On120FPSButtonClick()
        {
            this.UpdateFPSSelector(2f, 120);
        }

        private void OnUpdateLanguage()
        {
            int index = this._dropdownLanguage.value;
            GamePreferences.LanguageIndex = index;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }

        private void UpdateFPSSelector(float index, int fps)
        {
            this._tweenFPSSelector?.Kill();
            this._tweenFPSSelector = DOTween.To(() => this._fpsSelectorAnchor, x => this._fpsSelectorAnchor = x, index, 0.3f)
                    .OnUpdate(() =>
                    {
                        this._fpsSelector.anchorMin = new Vector2(this._fpsSelectorAnchor / 3f, 0);
                        this._fpsSelector.anchorMax = new Vector2((this._fpsSelectorAnchor + 1f) / 3f, 1f);
                    });
            GamePreferences.FPS = fps;
        }
    }
}