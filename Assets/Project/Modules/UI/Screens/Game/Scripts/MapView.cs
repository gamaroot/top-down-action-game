using System;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using Utils;

namespace Game
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtEnemiesDefeated;
        [SerializeField] private TextMeshProUGUI _txtHighestCombo;
        [SerializeField] private TextMeshProUGUI _txtTimeElapsed;

        private GameManager _gameManager;

        private void Awake()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();
        }

        private void OnEnable()
        {
            this._txtEnemiesDefeated.text = Statistics.Instance.CurrentRunData.TotalEnemiesKilled.ToString();
            this._txtHighestCombo.text = Statistics.Instance.CurrentRunData.HighestCombo.ToString();
            this._gameManager.OnMapVisibilityChange(true);
        }

        private void OnDisable()
        {
            this._gameManager.OnMapVisibilityChange(false);
        }

        private void Update()
        {
            double totalSeconds = (DateTime.Now - Statistics.Instance.LevelStartTime).TotalSeconds;
            int minutes = (int)totalSeconds / 60;
            int seconds = (int)totalSeconds % 60;
            int milliseconds = (int)((totalSeconds - (int)totalSeconds) * 1000);

            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            this._txtTimeElapsed.text = formattedTime;
        }

        // Called by the Animator
        private void OnHideAnimationEnd()
        {
            base.gameObject.SetActive(false);
        }
    }
}