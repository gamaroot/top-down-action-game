using ScreenNavigation;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utils;
using UnityEngine.Localization.Settings;

namespace Game
{
    public class DebugScreenEvents : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _spawnLoopInterval = 5f;

        [Header("Spawns")]
        [SerializeField] private TMP_Dropdown _dropdownSpawnableEnemies;
        [SerializeField] private TMP_Dropdown _dropdownSpawnableTraps;
        [SerializeField] private Toggle _toggleSpawnLoop;
        [SerializeField] private LayerMask _spawnLayerMaskToAvoid;

        [Header("Player")]
        [SerializeField] private GameObject _buttonPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerRespawn;

        [Header("HUD")]
        [SerializeField] private string _heartIcon;

        [SerializeField] private Slider _sliderHp;
        [SerializeField] private TextMeshProUGUI _txtHealthBack;
        [SerializeField] private TextMeshProUGUI _txtHealthFront;

        [SerializeField] private Slider _sliderXp;
        [SerializeField] private TextMeshProUGUI _txtLevelBack;
        [SerializeField] private TextMeshProUGUI _txtLevelFront;

        [Header("Others")]
        [SerializeField] private GameObject _iconGamepad;
        [SerializeField] private List<GameObject> _waypoints;

        private int _currentActiveCamera;
        private readonly CinemachineCamera[] _cameras = new CinemachineCamera[2];

        private string _baseLevelText;
        private GameManager _gameManager;
        private PlayerHealthController _player;
        private readonly DebugUtils _debugUtils = new();

        private void OnValidate()
        {
            this.LoadDropdown<SpawnTypeEnemy>(this._dropdownSpawnableEnemies);
            this.LoadDropdown<SpawnTypeTrap>(this._dropdownSpawnableTraps);
        }

        private void Awake()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();

            // This is a debug scene, so we can safely not consider the performance here
            this._cameras[0] = GameObject.FindGameObjectWithTag(Tags.PlayerCamera.ToString()).GetComponent<CinemachineCamera>();
            this._cameras[1] = GameObject.FindGameObjectWithTag(Tags.StageCamera.ToString()).GetComponent<CinemachineCamera>();

            this._player = new CrossSceneReference().GetObjectByType<PlayerHealthController>();

            this._gameManager.OnPlayerHealthUpdateListener += this.OnHealthUpdated;
            this._gameManager.OnPlayerXpUpdateListener += this.OnXpUpdated;
            this._gameManager.OnPlayerLevelUpdateListener += this.OnLevelUpdated;

            this._baseLevelText = LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationKeys.SCREEN_GAME,
                                                                                         LocalizationKeys.SCREEN_GAME_TXT_LEVEL);
            this.OnLevelUpdated(this._gameManager.GameState.PlayerState.Level);
        }

        private void OnDestroy()
        {
            if (this._player)
                this._player.gameObject.SetActive(false);
        }

        private void Update()
        {
            this._iconGamepad.SetActive(Gamepad.current != null);
            this.UpdatePlayerHealButton();
        }

        public void OnCameraButtonClick()
        {
            this._cameras[this._currentActiveCamera].Priority = 0;

            if (++this._currentActiveCamera >= this._cameras.Length)
            {
                this._currentActiveCamera = 0;
            }
            CinemachineCamera nextCamera = this._cameras[this._currentActiveCamera];
            nextCamera.Priority = 1;
        }

        public void OnSpawnEnemyButtonClick()
        {
            SpawnTypeEnemy spawnType;
            if (this._dropdownSpawnableEnemies.value == 0)
                spawnType = (SpawnTypeEnemy)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpawnTypeEnemy)).Length);
            else
                spawnType = (SpawnTypeEnemy)(this._dropdownSpawnableEnemies.value - 1);

            BehaviorGraphAgent spawn = SpawnablePool.SpawnEnemy<BehaviorGraphAgent>(spawnType);
            spawn.transform.position = this._debugUtils.GetRandomCircularPosition(this._player.transform.position, 15f, 10f, 5f, this._spawnLayerMaskToAvoid);
            spawn.gameObject.SetActive(true);
            spawn.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);
        }

        public void OnSpawnLoopToggleClick()
        {
            if (this._toggleSpawnLoop.isOn)
                this.InvokeRepeating(nameof(this.OnSpawnEnemyButtonClick), 0f, this._spawnLoopInterval);
            else
                this.CancelInvoke(nameof(this.OnSpawnEnemyButtonClick));
        }

        public void OnSpawnTrapButtonClick()
        {
            SpawnTypeTrap spawnType;
            if (this._dropdownSpawnableTraps.value == 0)
                spawnType = (SpawnTypeTrap)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpawnTypeTrap)).Length);
            else
                spawnType = (SpawnTypeTrap)(this._dropdownSpawnableTraps.value - 1);

            Transform spawn = SpawnablePool.SpawnTrap<Transform>(spawnType);
            spawn.position = this._debugUtils.GetRandomCircularPosition(Vector3.zero, 15f, 10f, 5f, this._spawnLayerMaskToAvoid);

            spawn.gameObject.SetActive(true);
        }

        public void OnPlayerHealButtonClick()
        {
            if (this._player.IsDead)
                this._player.CallNonPublicMethod("OnRespawn");
            else
                this._player.CallNonPublicMethod("RecoverHealth", new object[] { this._player.MissingHealth });
        }

        private void LoadDropdown<T>(TMP_Dropdown dropdown)
        {
            dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new()
            {
                new TMP_Dropdown.OptionData("RANDOM")
            };
            foreach (string spawnTypeName in Enum.GetNames(typeof(T)))
            {
                options.Add(new TMP_Dropdown.OptionData(spawnTypeName));
            }
            dropdown.AddOptions(options);
        }

        private void UpdatePlayerHealButton()
        {
            if (this._player.IsDead)
            {
                this._buttonPlayerHeal.SetActive(true);
                this._buttonLabelPlayerHeal.SetActive(false);
                this._buttonLabelPlayerRespawn.SetActive(true);
            }
            else if (!this._player.IsFullHealth)
            {
                this._buttonPlayerHeal.SetActive(true);
                this._buttonLabelPlayerHeal.SetActive(true);
                this._buttonLabelPlayerRespawn.SetActive(false);
            }
            else
            {
                this._buttonPlayerHeal.SetActive(false);
                this._buttonLabelPlayerHeal.SetActive(false);
                this._buttonLabelPlayerRespawn.SetActive(false);
            }
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