using ScreenNavigation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InGameSettingsScreenEvents : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Color _colorToggleOn;
        [SerializeField] private Color _colorToggleOff;

        [Header("Components")]
        [SerializeField] private Animator _animator;

        [SerializeField] private Image _toggleSound;
        [SerializeField] private Image _toggleMusic;

        private float _currentSoundVolume;
        private float _currentMusicVolume;

        private void Awake()
        {
            this._currentSoundVolume = GamePreferences.SoundVolume;
            this._currentMusicVolume = GamePreferences.MusicVolume;

            this.UpdateToggle(this._toggleSound, this._currentSoundVolume > 0);
            this.UpdateToggle(this._toggleMusic, this._currentMusicVolume > 0);

            SceneNavigator.Instance.ExecuteOnHideProcessStart(SceneID.INGAME_SETTINGS, this.HideScreen);
        }

        public void OnSoundToggleClick()
        {
            bool isOn = GamePreferences.SoundVolume > 0;
            isOn = !isOn;
            GamePreferences.SoundVolume = isOn ? this._currentSoundVolume : 0;

            this.UpdateToggle(this._toggleSound, isOn);
        }

        public void OnMusicToggleClick()
        {
            bool isOn = GamePreferences.MusicVolume > 0;
            isOn = !isOn;
            GamePreferences.MusicVolume = isOn ? this._currentMusicVolume : 0;

            this.UpdateToggle(this._toggleMusic, isOn);
        }

        public void OnEscapeButtonClick()
        {
            SceneNavigator.Instance.UnloadSceneAsync(SceneID.INGAME_SETTINGS);
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME, SceneID.HOME);
        }

        private void UpdateToggle(Image toggle, bool isOn)
        {
            toggle.color = isOn ? this._colorToggleOn : this._colorToggleOff;
        }

        private void HideScreen()
        {
            this._animator.SetBool(AnimationKeys.VISIBLE, false);
        }
    }
}