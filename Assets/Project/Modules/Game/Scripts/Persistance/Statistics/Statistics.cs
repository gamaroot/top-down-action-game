using System;
using UnityEngine;

namespace Game
{
    public class Statistics
    {
        public StatisticsData CurrentRunData { get; private set; }
        public StatisticsData LifetimeData { get; private set; }

        public static void Initialize()
        {
            Instance = new Statistics
            {
                CurrentRunData = new StatisticsData(),
                LifetimeData = new StatisticsData
                {
                    TotalEnemiesKilled = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_ENEMIES_KILLED_KEY),
                    TotalPlayerDeaths = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYER_DEATHS_KEY),
                    HighestCombo = PlayerPrefs.GetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY),
                    TotalPlayTimeInSeconds = float.Parse(PlayerPrefs.GetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, "0"))
                }
            };
        }
        public static Statistics Instance;

        public DateTime LevelStartTime { get; private set; }

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

            Debug.Log($"TotalPlayerDeaths -> Current: {this.CurrentRunData.TotalPlayerDeaths} / Lifetime: {this.LifetimeData.TotalPlayerDeaths}");


            PlayerPrefs.Save();
        }

        public void OnGameStart()
        {
            this.LevelStartTime = DateTime.Now;
        }

        public void OnGameOver(float totalXP)
        {
            this.UpdateTotalPlayTime();
            this.UpdateMaxXpInRun(totalXP);

            PlayerPrefs.Save();
        }

        public void OnComboFinished(int killStreak)
        {
            if (killStreak <= this.CurrentRunData.HighestCombo)
                return;

            this.CurrentRunData.HighestCombo = killStreak;

            if (killStreak > this.LifetimeData.HighestCombo)
            {
                this.LifetimeData.HighestCombo = killStreak;
            }
            PlayerPrefs.SetInt(PlayerPrefsKeys.STATISTICS_HIGHEST_COMBO_KEY, this.LifetimeData.HighestCombo);
            PlayerPrefs.Save();

            Debug.Log($"HighestCombo -> Current: {this.CurrentRunData.HighestCombo} / Lifetime: {this.LifetimeData.HighestCombo}");
        }

        private void UpdateTotalPlayTime()
        {
            double timeSinceLevelLoad = (DateTime.Now - this.LevelStartTime).TotalSeconds;
            this.CurrentRunData.TotalPlayTimeInSeconds = timeSinceLevelLoad;
            this.LifetimeData.TotalPlayTimeInSeconds += timeSinceLevelLoad;
            PlayerPrefs.SetString(PlayerPrefsKeys.STATISTICS_TOTAL_PLAYTIME_IN_SECONDS_KEY, this.LifetimeData.TotalPlayTimeInSeconds.ToString());

            Debug.Log($"TotalPlayTimeInSeconds -> Current: {this.CurrentRunData.TotalPlayTimeInSeconds} / Lifetime: {this.LifetimeData.TotalPlayTimeInSeconds}");
        }

        private void UpdateMaxXpInRun(float totalXP)
        {
            if (totalXP <= this.CurrentRunData.MaxXpInRun)
                return;

            this.CurrentRunData.MaxXpInRun = totalXP;

            if (totalXP > this.LifetimeData.MaxXpInRun)
            {
                this.LifetimeData.MaxXpInRun = totalXP;
            }
            PlayerPrefs.SetFloat(PlayerPrefsKeys.STATISTICS_TOTAL_MAX_XP_IN_RUN_KEY, this.LifetimeData.MaxXpInRun);

            Debug.Log($"MaxXpInRun -> Current: {this.CurrentRunData.MaxXpInRun} / Lifetime: {this.LifetimeData.MaxXpInRun}");
        }
    }
}