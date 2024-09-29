using DG.Tweening;
using ScreenNavigation;
using System.Collections;
using System.Collections.Generic;
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

        private readonly List<int> _toPlay = new();

        private int _playingMusic = -1;

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

        private void RandomizeMusic()
        {
            if (!this.IsMusicEnabled())
                return;

            this._audioSource.loop = false;

            if (this._audioSource.isPlaying)
            {
                this._audioSource.DOFade(0, AUDIO_FADE_DURATION)
                                 .OnComplete(this.PlayRandomTrack);
            } else {
                this.PlayRandomTrack();
            }
        }

        private void PlayRandomTrack()
        {
            if (this._toPlay.Count == 0)
            {
                for (int index = 0; index < this._playlist.Length; index++)
                {
                    if (index != this._playingMusic)
                        this._toPlay.Add(index);
                }
            }

            this._playingMusic = this._toPlay[Random.Range(0, this._toPlay.Count)];
            this._toPlay.Remove(_playingMusic);

            this._audioSource.clip = this._playlist[this._playingMusic];
            this._audioSource.Play();
            this._audioSource.DOFade(1f, AUDIO_FADE_DURATION);

            float remainingTime = this._audioSource.clip.length - this._audioSource.time;
            base.Invoke(nameof(this.RandomizeMusic), remainingTime);
        }

        private bool IsMusicEnabled()
        {
            return GamePreferences.MusicVolume > 0;
        }
    }
}
