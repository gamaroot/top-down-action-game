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

        [SerializeField] private GameObject _textCameraPlayer;
        [SerializeField] private GameObject _textCameraStage;

        [Header("Player")]
        [SerializeField] private GameObject _buttonPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerRespawn;

        [Header("Others")]
        [SerializeField] private GameObject _iconGamepad;
        [SerializeField] private List<GameObject> _waypoints;

        private int _currentActiveCamera;
        private readonly CinemachineCamera[] _cameras = new CinemachineCamera[2];

        private IGameManager _gameManager;
        private PlayerHealthController _player;
        private readonly DebugUtils _debugUtils = new();

        private void OnValidate()
        {
            this.LoadDropdown<SpawnTypeEnemy>(this._dropdownSpawnableEnemies);
            this.LoadDropdown<SpawnTypeTrap>(this._dropdownSpawnableTraps);
        }

        private void OnEnable()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<GameManager>();

            // This is a debug scene, so we can safely not consider the performance here
            this._cameras[0] = GameObject.FindGameObjectWithTag(Tags.PlayerCamera.ToString()).GetComponent<CinemachineCamera>();
            this._cameras[1] = GameObject.FindGameObjectWithTag(Tags.StageCamera.ToString()).GetComponent<CinemachineCamera>();

            this._player = new CrossSceneReference().GetObjectByType<PlayerHealthController>();
            this._player.OnReset();
        }

        private void OnDisable()
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

            bool isPlayerCamera = nextCamera.CompareTag(Tags.PlayerCamera.ToString());
            this._textCameraPlayer.SetActive(isPlayerCamera);
            this._textCameraStage.SetActive(!isPlayerCamera);
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
            {
                this.InvokeRepeating(nameof(this.OnSpawnEnemyButtonClick), 0f, this._spawnLoopInterval);
            }
            else
            {
                this.CancelInvoke(nameof(this.OnSpawnEnemyButtonClick));
            }
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

        public void OnGenerateMapButtonClick()
        {
            // Access the private field using reflection
            this._gameManager.GetType()
                             .GetMethod("GenerateMap")
                             .Invoke(this._gameManager, null);
        }

        public void OnPlayerHealButtonClick()
        {
            if (this._player.IsDead)
                this._player.OnRespawn();
            else
                this._player.RecoverHealth(this._player.MissingHealth);
        }

        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.DEBUG, SceneID.HOME);
            SpawnablePool.DisableAll();
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
    }
}