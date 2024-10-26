using DG.Tweening;
using ScreenNavigation;
using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
    public class BGM : MonoBehaviour
    {
        private const float AUDIO_FADE_DURATION = 1f;

        [HideInInspector]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _playlist;

        private void OnValidate()
        {
            if (this._audioSource == null)
                this._audioSource = base.GetComponent<AudioSource>();
        }

        private IEnumerator Start()
        {
            yield return new WaitWhile(() => SceneNavigator.Instance == null);

            GamePreferences.OnMusicVolumeChange = this.OnMusicPrefChange;

            if (GamePreferences.MusicVolume > 0)
            {
                SceneNavigator.Instance.AddListenerOnScreenStateChange((sceneID, state) =>
                {
                    if (state == SceneState.LOADED)
                    {
                        switch (sceneID)
                        {
                            case SceneID.HOME:
                                this.PlayMusic(BGMType.MENU);
                                break;

                                // TODO: Add more cases for other scenes
                        }
                    }
                });
            }
        }

        private void OnMusicPrefChange(float volume)
        {
            this._audioSource.volume = volume;

            if (volume > 0 && !this._audioSource.isPlaying)
                this._audioSource.Play();
            else if (volume == 0 && this._audioSource.isPlaying)
                this._audioSource.Pause();
        }

        private void PlayMusic(BGMType type)
        {
            if (!this.IsMusicEnabled())
                return;

            this._audioSource.loop = true;

            if (this._audioSource.isPlaying)
            {
                this._audioSource.DOFade(0, AUDIO_FADE_DURATION)
                                 .OnComplete(() => this.ChangeMusic(type));
            } else {
                this.ChangeMusic(type);
            }
        }

        private void ChangeMusic(BGMType type)
        {
            this._audioSource.clip = this._playlist[(int)type];
            this._audioSource.Play();
            this._audioSource.DOFade(1f, AUDIO_FADE_DURATION);
        }

        private bool IsMusicEnabled()
        {
            return GamePreferences.MusicVolume > 0;
        }
    }
}
