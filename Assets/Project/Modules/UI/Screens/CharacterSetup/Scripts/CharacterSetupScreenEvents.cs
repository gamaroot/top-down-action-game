using ScreenNavigation;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class CharacterSetupScreenEvents : MonoBehaviour
    {
        [Header("UI Controllers")]
        [SerializeField] private Slider[] _sliders; // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown
        [SerializeField] private Button[] _sliderNegativeButtons; // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown
        [SerializeField] private Button[] _sliderPositiveButtons; // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown
        [SerializeField] private Slider _sliderXP;

        [Header("UI Values")]
        [SerializeField] private TextMeshProUGUI _txtLevel;
        [SerializeField] private TextMeshProUGUI _txtXp;
        [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
        [SerializeField] private TextMeshProUGUI[] _statTexts; // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown

        private IGameManager _gameManager;
        private int[] _assignedStatPoints;
        private int _availableStatPoints;

        private readonly float[] _sliderValues = new float[4]; // Store the values of the 4 sliders

        private InputController _input;

        private void Awake()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();
            this._assignedStatPoints = this._gameManager.GameState.PlayerState.ExtraStats;
            this._availableStatPoints = this._gameManager.PlayerConfig.GetTotalStatsPoints(this._gameManager.GameState.PlayerState.Level);

            this.ConfigureUI();

            this._input = new InputController();
            this._input.UI.Close.performed += _ => this.OnCloseButtonClick();
        }

        private void Start()
        {
            for (int index = 0; index < this._sliders.Length; index++)
            {
                int indexRef = index; // Capture loop variable for delegate
                this._sliders[index].onValueChanged.AddListener(value => this.OnSliderChange(indexRef, value));
            }
            this.UpdateAssignButtons();
        }

        private void OnEnable()
        {
            this._input.Enable();
        }

        private void OnDisable()
        {
            this._input.Disable();
        }

        public void OnStartButtonClick()
        {
            this._gameManager.OnPlayerStateUpdated(new PlayerState
            {
                Level = this._gameManager.GameState.PlayerState.Level,
                XP = this._gameManager.GameState.PlayerState.XP,
                ExtraStats = this._assignedStatPoints
            });
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.CHARACTER_SETUP, SceneID.GAME);
        }

        // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown
        public void OnAddStatButtonClick(int statIndex)
        {
            this._sliders[statIndex].value++;
        }

        // 0: MaxHealth, 1: MovementSpeed, 2: DashCooldown, 3: ParryCooldown
        public void OnRemoveStatButtonClick(int statIndex)
        {
            this._sliders[statIndex].value--;
        }

        public void OnCloseButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.CHARACTER_SETUP, SceneID.HOME);
        }

        private void OnSliderChange(int statIndex, float value)
        {
            if (value == this._sliderValues[statIndex]) return;

            int diff = (int)(value - this._sliderValues[statIndex]);

            if (diff > this._availableStatPoints)
            {
                this._sliders[statIndex].value = this._sliderValues[statIndex];
                this.UpdateAssignButtons();
                return;
            }

            this._availableStatPoints -= diff;
            this._assignedStatPoints[statIndex] += diff;
            this._txtAvailablePoints.text = this._availableStatPoints.ToString();

            this._sliderValues[statIndex] = value;
            this.UpdateStatText(statIndex);

            this.UpdateAssignButtons();
        }

        private void ConfigureUI()
        {
            this._txtLevel.text = this._gameManager.GameState.PlayerState.Level.ToString();

            float currentXp = this._gameManager.GameState.PlayerState.XP;
            float xpToNextLevel = this._gameManager.PlayerConfig.GetXpToNextLevel(this._gameManager.GameState.PlayerState.Level);

            this._sliderXP.maxValue = xpToNextLevel;
            this._sliderXP.value = currentXp;
            this._txtXp.text = $"{currentXp} / {xpToNextLevel} XP";

            int maxSliderValue = this._availableStatPoints;
            for (int index = 0; index < this._sliders.Length; index++)
            {
                int assignedPoints = this._assignedStatPoints[index];

                float maxPoints = this._gameManager.PlayerConfig.GetMaxPointsForStatsByIndex(index);
                this._sliders[index].maxValue = Mathf.Clamp(maxSliderValue, 0, maxPoints);
                this._sliders[index].value = assignedPoints;
                this._sliderValues[index] = this._sliders[index].value;

                this._availableStatPoints -= assignedPoints;

                this.UpdateStatText(index);
            }
            this._txtAvailablePoints.text = this._availableStatPoints.ToString();
        }

        private void UpdateStatText(int statIndex)
        {
            (float baseStat, float statsPerPoints) = this._gameManager.PlayerConfig.GetStatsConfigByIndex(statIndex);
            float points = this._sliders[statIndex].value;

            float statValue = baseStat + (points * statsPerPoints);
            this._statTexts[statIndex].text = statValue.ToString();
        }

        private void UpdateAssignButtons()
        {
            for (int index = 0; index < this._sliderNegativeButtons.Length; index++)
            {
                this._sliderPositiveButtons[index].interactable = this._availableStatPoints > 0 && this._sliders[index].value < this._sliders[index].maxValue;
                this._sliderNegativeButtons[index].interactable = this._assignedStatPoints[index] > 0;
            }
        }
    }
}