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

        private static AudioClip[] _playlist;
        private static AudioSource _audioSource;

        private static BGMType _currentMusic;

        private void Awake()
        {
            _playlist = new AudioClip[]
            {
                Resources.Load<AudioClip>($"bgm_{BGMType.GAMEPLAY}".ToLower()),
                Resources.Load<AudioClip>($"bgm_{BGMType.BOSS}".ToLower()),
            };
            _audioSource = base.GetComponent<AudioSource>();
        }

        private IEnumerator Start()
        {
            yield return new WaitWhile(() => SceneNavigator.Instance == null);

            GamePreferences.OnMusicVolumeChange = this.OnMusicPrefChange;

            SceneNavigator.Instance.AddListenerOnScreenStateChange((sceneID, state) =>
            {
                if (sceneID != SceneID.GAME || GamePreferences.MusicVolume == 0)
                    return;

                if (state == SceneState.LOADING)
                {
                    PlayMusic(BGMType.GAMEPLAY);
                }
                else if (state == SceneState.UNLOADED)
                {
                    this.PauseMusic();
                }
            });
        }

        private void OnMusicPrefChange(float volume)
        {
            _audioSource.volume = volume;

            if (_currentMusic == BGMType.NONE)
                return;

            if (volume > 0 && !_audioSource.isPlaying)
                _audioSource.Play();
            else if (volume == 0 && _audioSource.isPlaying)
                _audioSource.Pause();
        }

        public static void PlayMusic(BGMType type)
        {
            if (IsMusicMuted() || _currentMusic == type)
                return;

            _audioSource.loop = true;

            _audioSource.DOKill();
            if (_audioSource.isPlaying)
            {
                _audioSource.DOFade(0, AUDIO_FADE_DURATION)
                                 .OnComplete(() => ChangeMusic(type));
            } else {
                ChangeMusic(type);
            }
            _currentMusic = type;
        }

        private void PauseMusic()
        {
            _currentMusic = BGMType.NONE;

            _audioSource.DOKill();
            _audioSource.DOFade(0, AUDIO_FADE_DURATION)
                             .OnComplete(_audioSource.Pause);
        }

        private static void ChangeMusic(BGMType type)
        {
            _audioSource.clip = _playlist[(int)type];
            _audioSource.Play();

            _audioSource.DOFade(1f, AUDIO_FADE_DURATION);
        }

        private static bool IsMusicMuted()
        {
            return GamePreferences.MusicVolume == 0;
        }
    }
}
