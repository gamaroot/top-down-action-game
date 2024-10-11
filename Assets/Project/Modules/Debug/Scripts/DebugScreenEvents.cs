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

        [Header("Cameras")]
        [SerializeField] private CinemachineCamera[] _cameras;
        [SerializeField] private TextMeshProUGUI _textCurrentCamera;

        [Header("Player")]
        [SerializeField] private PlayerHealthController _player;
        [SerializeField] private GameObject _buttonPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerHeal;
        [SerializeField] private GameObject _buttonLabelPlayerRespawn;

        [Header("Others")]
        [SerializeField] private GameObject _iconGamepad;
        [SerializeField] private List<GameObject> _waypoints;

        private int _currentActiveCamera;
        private ToastHandler _toastHandler;
        private readonly DebugUtils _debugUtils = new();

        private void OnValidate()
        {
            this.LoadDropdown<SpawnTypeEnemy>(this._dropdownSpawnableEnemies);
            this.LoadDropdown<TrapSpawnType>(this._dropdownSpawnableTraps);
        }

        private void Awake()
        {
            this._toastHandler = new CrossSceneReference().GetObjectByType<ToastHandler>();
            this._player.RespawnListener = () => this._toastHandler.Show("You Respawned!");
            this._player.DeathListener = () => this._toastHandler.Show("You died!");
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

            this._textCurrentCamera.text = nextCamera.name.Replace(" Camera", "");
        }

        public void OnSpawnEnemyButtonClick()
        {
            SpawnTypeEnemy spawnType;
            if (this._dropdownSpawnableEnemies.value == 0)
                spawnType = (SpawnTypeEnemy)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpawnTypeEnemy)).Length);
            else
                spawnType = (SpawnTypeEnemy)(this._dropdownSpawnableEnemies.value + 1);
            
            Transform spawn = SpawnablePool.SpawnEnemy<Transform>(spawnType);
            spawn.position = this._debugUtils.GetRandomCircularPosition(this._player.transform.position, 15f, 10f);

            BehaviorGraphAgent enemy = spawn.GetComponent<BehaviorGraphAgent>();
            spawn.gameObject.SetActive(true);
            enemy.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);
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
            TrapSpawnType spawnType;
            if (this._dropdownSpawnableTraps.value == 0)
                spawnType = (TrapSpawnType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TrapSpawnType)).Length);
            else
                spawnType = (TrapSpawnType)(this._dropdownSpawnableTraps.value + 1);

            Transform spawn = SpawnablePool.SpawnTrap<Transform>(spawnType);
            spawn.position = this._debugUtils.GetRandomCircularPosition(Vector3.zero, 15f, 10f);

            spawn.gameObject.SetActive(true);
        }

        public void OnPlayerHealButtonClick()
        {
            if (this._player.IsDead)
                this._player.Respawn();
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