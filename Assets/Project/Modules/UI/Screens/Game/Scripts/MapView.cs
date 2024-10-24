using TMPro;
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
            int totalSeconds = (int)Time.timeSinceLevelLoad;
            string formattedMinutesAndSeconds = string.Format("{0:00}:{1:00}", (int)totalSeconds / 60, (int)totalSeconds % 60);
            this._txtTimeElapsed.text = formattedMinutesAndSeconds;
        }

        // Called by the Animator
        private void OnHideAnimationEnd()
        {
            base.gameObject.SetActive(false);
        }
    }
}