using ScreenNavigation;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Game
{
    public class GameOverScreenEvents : MonoBehaviour
    {
        [SerializeField] private RectTransform _mapDisplay;
        [SerializeField] private CanvasRoomDisplay _roomDisplayPrefab;

        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private TextMeshProUGUI _txtEnemiesDefeated;
        [SerializeField] private TextMeshProUGUI _txtHighestCombo;
        [SerializeField] private TextMeshProUGUI _txtTimeElapsed;

        private void Awake()
        {
            StatisticsData data = Statistics.Instance.CurrentRunData;

            (int previousLevel, int currentLevel, IRoom[] rooms) = ((int, int, IRoom[]))SceneNavigator.Instance.GetSceneParams(SceneID.GAME_OVER);

            this.CreateMap(rooms);

            this.UpdateEnemiesDefeated(data);
            this.UpdateHighestCombo(data);
            this.UpdateRunTime(data);
            this.UpdateLevelProgress(data, previousLevel, currentLevel);
        }

        public void OnPlayButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME_OVER, SceneID.CHARACTER_SETUP);
        }

        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME_OVER, SceneID.HOME);
        }

        private void CreateMap(IRoom[] rooms)
        {
            var displays = new CanvasRoomDisplay[rooms.Length];
            for (int index = 0; index < rooms.Length; index++)
            {
                CanvasRoomDisplay room = Instantiate(this._roomDisplayPrefab);
                room.transform.SetParent(this._mapDisplay);
                room.Setup(rooms[index]);
                displays[index] = room;
            }

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (CanvasRoomDisplay display in displays)
            {
                var corners = new Vector3[4];
                display.RectTransform.GetWorldCorners(corners);

                // Update bounds
                minX = Mathf.Min(minX, corners[0].x);
                minY = Mathf.Min(minY, corners[0].y);
                maxX = Mathf.Max(maxX, corners[2].x);
                maxY = Mathf.Max(maxY, corners[2].y);
            }
            var boundingCenter = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
            Vector3 canvasCenter = this._mapDisplay.position;

            // Calculate the offset needed to center the bounding box
            Vector3 offset = canvasCenter - boundingCenter;

            foreach (CanvasRoomDisplay display in displays)
            {
                display.transform.position += offset;
            }
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

        private void UpdateLevelProgress(StatisticsData data, int previousLevel, int currentLevel)
        {
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