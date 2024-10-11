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

            this._textVersion.text = Application.version.ToString();
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

        private void OnUpdateLanguage()
        {
            int index = this._dropdownLanguage.value;
            GamePreferences.LanguageIndex = index;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}