using TMPro;
using UnityEngine;

namespace Game
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtEnemiesDefeated;
        [SerializeField] private TextMeshProUGUI _txtHighestCombo;
        [SerializeField] private TextMeshProUGUI _txtTimeElapsed;

        private void OnEnable()
        {
            this._txtEnemiesDefeated.text = Statistics.Instance.TotalEnemiesKilled.ToString();
            this._txtHighestCombo.text = Statistics.Instance.HighestCombo.ToString();
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