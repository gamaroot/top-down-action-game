using UnityEngine;

namespace Game
{
    public class Statistics
    {
        public int TotalEnemiesKilled { get; private set; }
        public int TotalPlayerDeaths { get; private set; }
        public int HighestCombo { get; private set; }
        public int MaxXpInRun { get; private set; }
        public float TotalPlayTimeInSeconds { get; private set; }

        public static void Initialize()
        {
            Instance = new Statistics
            {
                TotalEnemiesKilled = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY),
                TotalPlayerDeaths = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYER_DEATHS_KEY),
                TotalPlayTimeInSeconds = float.Parse(PlayerPrefs.GetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, "0")),
                HighestCombo = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY)
            };
        }
        public static Statistics Instance;

        private Statistics() { }

        public void OnEnemyKilled()
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY, ++this.TotalEnemiesKilled);
            PlayerPrefs.Save();
        }

        public void OnPlayerDeath()
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYER_DEATHS_KEY, ++this.TotalPlayerDeaths);
            PlayerPrefs.Save();
        }

        public void OnGameQuit()
        {
            this.TotalPlayTimeInSeconds += Time.realtimeSinceStartup;
            PlayerPrefs.SetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, this.TotalPlayTimeInSeconds.ToString());
            PlayerPrefs.Save();
        }

        public void OnMatchEnd(int totalXP)
        {
            if (totalXP <= this.MaxXpInRun) return;

            this.MaxXpInRun = totalXP;

            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_MAX_XP_IN_RUN_KEY, this.MaxXpInRun);
            PlayerPrefs.Save();
        }

        public void OnComboFinished(int killStreak)
        {
            if (killStreak <= this.HighestCombo) return;

            this.HighestCombo = killStreak;

            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY, this.HighestCombo);
            PlayerPrefs.Save();
        }

        public void Reset()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY);
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY);
        }
    }
}