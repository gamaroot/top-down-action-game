using UnityEngine;

namespace Game
{
    public class Statistics
    {
        public StatisticsData CurrentRunData { get; private set; }
        public StatisticsData LifetimeData { get; private set; }
        public float TotalPlayTimeInSeconds { get; private set; }

        public static void Initialize()
        {
            Instance = new Statistics
            {
                CurrentRunData = new StatisticsData(),
                LifetimeData = new StatisticsData
                {
                    TotalEnemiesKilled = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY),
                    TotalPlayerDeaths = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYER_DEATHS_KEY),
                    HighestCombo = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY)
                },
                TotalPlayTimeInSeconds = float.Parse(PlayerPrefs.GetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, "0")),
            };
        }
        public static Statistics Instance;

        private Statistics() { }

        public void OnEnemyKilled()
        {
            this.CurrentRunData.TotalEnemiesKilled++;
            this.LifetimeData.TotalEnemiesKilled ++;
            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY, this.LifetimeData.TotalEnemiesKilled);
            PlayerPrefs.Save();

            Debug.Log($"TotalEnemiesKilled -> Current: {this.CurrentRunData.TotalEnemiesKilled} / Lifetime: {this.LifetimeData.TotalEnemiesKilled}");
        }

        public void OnPlayerDeath()
        {
            this.CurrentRunData.TotalPlayerDeaths++;
            this.LifetimeData.TotalPlayerDeaths++;
            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYER_DEATHS_KEY, this.LifetimeData.TotalPlayerDeaths);
            PlayerPrefs.Save();

            Debug.Log($"TotalPlayerDeaths -> Current: {this.CurrentRunData.TotalPlayerDeaths} / Lifetime: {this.LifetimeData.TotalPlayerDeaths}");
        }

        public void OnGameQuit()
        {
            this.TotalPlayTimeInSeconds += Time.realtimeSinceStartup;
            PlayerPrefs.SetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, this.TotalPlayTimeInSeconds.ToString());
            PlayerPrefs.Save();

            Debug.Log($"TotalPlayTimeInSeconds -> Lifetime: {this.TotalPlayTimeInSeconds}");
        }

        public void OnMatchEnd(int totalXP)
        {
            if (totalXP <= this.CurrentRunData.MaxXpInRun)
                return;

            this.CurrentRunData.MaxXpInRun = totalXP;

            if (totalXP <= this.LifetimeData.MaxXpInRun)
                return;

            this.LifetimeData.MaxXpInRun = totalXP;

            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_TOTAL_MAX_XP_IN_RUN_KEY, this.LifetimeData.MaxXpInRun);
            PlayerPrefs.Save();

            Debug.Log($"MaxXpInRun -> Current: {this.CurrentRunData.MaxXpInRun} / Lifetime: {this.LifetimeData.MaxXpInRun}");
        }

        public void OnComboFinished(int killStreak)
        {
            if (killStreak <= this.CurrentRunData.HighestCombo)
                return;

            this.CurrentRunData.HighestCombo = killStreak;

            if (killStreak <= this.LifetimeData.HighestCombo)
                return;

            this.LifetimeData.HighestCombo = killStreak;

            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY, this.LifetimeData.HighestCombo);
            PlayerPrefs.Save();

            Debug.Log($"HighestCombo -> Current: {this.CurrentRunData.HighestCombo} / Lifetime: {this.LifetimeData.HighestCombo}");
        }
    }
}