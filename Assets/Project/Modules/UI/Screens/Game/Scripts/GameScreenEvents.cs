using ScreenNavigation;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class GameScreenEvents : MonoBehaviour
    {
        [SerializeField] private string _heartIcon;

        [SerializeField] private Slider _sliderHp;
        [SerializeField] private TextMeshProUGUI _txtHealthBack;
        [SerializeField] private TextMeshProUGUI _txtHealthFront;

        [SerializeField] private Slider _sliderXp;
        [SerializeField] private TextMeshProUGUI _txtLevelBack;
        [SerializeField] private TextMeshProUGUI _txtLevelFront;

        private IGameManager _gameManager;
        private string _baseLevelText;

        private void Awake()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();

            this._gameManager.OnPlayerHealthUpdateListener += this.OnHealthUpdated;
            this._gameManager.OnPlayerXpUpdateListener += this.OnXpUpdated;
            this._gameManager.OnPlayerLevelUpdateListener += this.OnLevelUpdated;

            this._baseLevelText = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationKeys.SCREEN_GAME, 
                                                                                         LocalizationKeys.SCREEN_GAME_TXT_LEVEL);
            this.OnLevelUpdated(this._gameManager.GameState.PlayerState.Level);
        }

        private void OnXpUpdated(float xp, float xpToNextLevel)
        {
            this._sliderXp.maxValue = xpToNextLevel;
            this._sliderXp.value = xp;
        }

        private void OnHealthUpdated(int currentHealth, int maxHealth)
        {
            this._sliderHp.maxValue = maxHealth;
            this._sliderHp.value = currentHealth;

            string maxHearts = string.Empty;
            string currentHearts = string.Empty;
            for (int index = 0; index < maxHealth; index++)
            {
                maxHearts += this._heartIcon;

                if (index < currentHealth)
                    currentHearts += this._heartIcon;
            }

            this._txtHealthBack.text = maxHearts;
            this._txtHealthFront.text = currentHearts;
        }

        private void OnLevelUpdated(int level)
        {
            string text = string.Format(this._baseLevelText, level);

            this._txtLevelBack.text = text;
            this._txtLevelFront.text = text;
        }
    }
}