using ScreenNavigation;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Game
{
    public class GameOverScreenEvents : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private TextMeshProUGUI _txtEnemiesDefeated;
        [SerializeField] private TextMeshProUGUI _txtHighestCombo;
        [SerializeField] private TextMeshProUGUI _txtTimeElapsed;

        private void Awake()
        {
            StatisticsData data = Statistics.Instance.CurrentRunData;

            this.UpdateEnemiesDefeated(data);
            this.UpdateHighestCombo(data);
            this.UpdateRunTime(data);
            this.UpdateLevelProgress(data);
        }

        public void OnPlayButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME_OVER, SceneID.CHARACTER_SETUP);
        }

        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME_OVER, SceneID.HOME);
        }

        private void UpdateEnemiesDefeated(StatisticsData data)
        {
            string textEnemiesDefeated = this.GetLocalizedText(LocalizationKeys.TXT_ENEMIES_DEFEATED);
            this._txtEnemiesDefeated.text = string.Format(textEnemiesDefeated, data.TotalEnemiesKilled.ToString());
        }

        private void UpdateHighestCombo(StatisticsData data)
        {
            string textHighestCombo = this.GetLocalizedText(LocalizationKeys.TXT_HIGHEST_COMBO);
            this._txtHighestCombo.text = string.Format(textHighestCombo, data.HighestCombo.ToString());
        }

        private void UpdateRunTime(StatisticsData data)
        {
            double totalSeconds = data.TotalPlayTimeInSeconds;
            int minutes = (int)totalSeconds / 60;
            int seconds = (int)totalSeconds % 60;
            int milliseconds = (int)((totalSeconds - (int)totalSeconds) * 1000);

            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            string localizedText = this.GetLocalizedText(LocalizationKeys.TXT_RUN_TIME);

            this._txtTimeElapsed.text = string.Format(localizedText, formattedTime);
        }

        private void UpdateLevelProgress(StatisticsData data)
        {
            (int previousLevel, int currentLevel) = ((int, int))SceneNavigator.Instance.GetSceneParams(SceneID.GAME_OVER);

            string txtLevel;
            bool hasLeveledUp = currentLevel > previousLevel;
            if (hasLeveledUp)
            {
                txtLevel = this.GetLocalizedText(LocalizationKeys.TXT_LEVEL_INCREASED);
                txtLevel = string.Format(txtLevel, previousLevel, currentLevel, data.MaxXpInRun);
            }
            else
            {
                txtLevel = this.GetLocalizedText(LocalizationKeys.TXT_LEVEL);
                txtLevel = string.Format(txtLevel, currentLevel, data.MaxXpInRun);
            }
            this._txtLevel.text = txtLevel;
        }

        private string GetLocalizedText(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationKeys.SCREEN_GAME_OVER, key);
        }
    }
}