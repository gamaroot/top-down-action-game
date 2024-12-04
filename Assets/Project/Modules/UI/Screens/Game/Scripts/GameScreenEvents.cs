using DG.Tweening;
using ScreenNavigation;
using System;
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

        [SerializeField] private TextMeshProUGUI _txtHealthBack;
        [SerializeField] private TextMeshProUGUI _txtHealthFront;

        [SerializeField] private Slider _sliderXp;
        [SerializeField] private TextMeshProUGUI _txtLevelBack;
        [SerializeField] private TextMeshProUGUI _txtLevelFront;

        [SerializeField] private Animator _btnSettingsAnimator;
        [SerializeField] private Animator _bossHpBarAnimator;

        [SerializeField] private Slider _bossHpSlider;

        private IGameManager _gameManager;
        private string _baseLevelText;

        private InputController _input;

        private void Awake()
        {
            this._input = new InputController();
            this._input.UI.Start.performed += _ => this.OnMenuButtonClick();

            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();

            this._gameManager.OnPlayerHealthUpdateListener = this.OnHealthUpdated;
            this._gameManager.OnPlayerXpUpdateListener = this.OnXpUpdated;
            this._gameManager.OnPlayerLevelUpdateListener = this.OnLevelUpdated;

            GameManager.OnEnemySpawnListener = this.OnEnemySpawnListener;
            GameManager.OnEnemyHealthUpdateListener = this.OnEnemyHealthUpdated;

            this._baseLevelText = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationKeys.SCREEN_GAME, 
                                                                                         LocalizationKeys.TXT_LEVEL);
            this.OnLevelUpdated(this._gameManager.GameState.PlayerState.Level);
        }

        private void OnEnable()
        {
            this._input.Enable();
        }

        private void OnDisable()
        {
            this._input.Disable();
        }

        private void OnXpUpdated(float xp, float xpToNextLevel)
        {
            this._sliderXp.maxValue = xpToNextLevel;
            this._sliderXp.value = xp;
        }

        private void OnHealthUpdated(int currentHealth, int maxHealth)
        {
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

        public void OnMenuButtonClick()
        {
            bool visible = !this._btnSettingsAnimator.GetBool(AnimationKeys.VISIBLE);
            this._btnSettingsAnimator.SetBool(AnimationKeys.VISIBLE, visible);

            if (visible)
                SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.INGAME_SETTINGS);
            else
                SceneNavigator.Instance.UnloadSceneAsync(SceneID.INGAME_SETTINGS);

            this._gameManager.SetInputEnabled(!visible);
        }

        private void OnEnemySpawnListener(SpawnTypeEnemy type, float maxHealth, float health)
        {
            if (type != SpawnTypeEnemy.BOSS)
                return;

            this._bossHpSlider.value = 0;
            this._bossHpSlider.maxValue = maxHealth;

            this.UpdateBossHpDisplay(health);
        }

        private void OnEnemyHealthUpdated(SpawnTypeEnemy type, float maxHealth, float health)
        {
            if (type != SpawnTypeEnemy.BOSS)
                return;
            
            this.UpdateBossHpDisplay(health);
        }

        private void UpdateBossHpDisplay(float health)
        {
            this._bossHpSlider.DOKill();
            this._bossHpSlider.DOValue(health, 1f);

            this._bossHpBarAnimator.SetBool(AnimationKeys.VISIBLE, health > 0);
        }
    }
}