using System;
using UnityEngine;

namespace Game
{
    public class GamePreferences
    {
        private const string SOUND_LEVEL_KEY = "SOUND_LEVEL",
                             MUSIC_LEVEL_KEY = "MUSIC_LEVEL",
                             LANGUAGE_KEY = "LANGUAGE",
                             FPS_KEY = "FPS";

        public static Action<float> OnMusicVolumeChange;

        public static float SoundVolume
        {
            get => PlayerPrefs.GetFloat(SOUND_LEVEL_KEY, 0.7f);
            set
            {
                PlayerPrefs.SetFloat(SOUND_LEVEL_KEY, value);
                PlayerPrefs.Save();
            }
        }

        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MUSIC_LEVEL_KEY, 1f);
            set
            {
                PlayerPrefs.SetFloat(MUSIC_LEVEL_KEY, value);
                PlayerPrefs.Save();

                OnMusicVolumeChange.Invoke(value);
            }
        }

        public static int FPS
        {
            get => PlayerPrefs.GetInt(FPS_KEY, 120);
            set
            {
                Application.targetFrameRate = value;

                PlayerPrefs.SetInt(FPS_KEY, value);
                PlayerPrefs.Save();
            }
        }

        public static int LanguageIndex
        {
            get => PlayerPrefs.GetInt(LANGUAGE_KEY);
            set
            {
                PlayerPrefs.SetInt(LANGUAGE_KEY, value);
                PlayerPrefs.Save();
            }
        }
    }
}