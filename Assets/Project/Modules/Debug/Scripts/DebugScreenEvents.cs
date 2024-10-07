using ScreenNavigation;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Utils;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Game
{
    public class DebugScreenEvents : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _spawnLoopInterval = 5f;

        [Header("Components")]
        [SerializeField] private GameObject _iconGamepad;
        [SerializeField] private TMP_Dropdown _dropdownSpawnableEnemies;
        [SerializeField] private TMP_Dropdown _dropdownSpawnableTraps;
        [SerializeField] private Toggle _toggleSpawnLoop;
        [SerializeField] private CinemachineCamera[] _cameras;
        [SerializeField] private TextMeshProUGUI _textCurrentCamera;
        [SerializeField] private List<GameObject> _waypoints;
        [SerializeField, ReadOnly] private GameObject _player;

        private int _currentActiveCamera;
        private readonly DebugUtils _debugUtils = new();

        private void OnValidate()
        {
            if (this._player == null)
                this._player = GameObject.FindWithTag(Tags.Player.ToString());

            this.LoadDropdown<SpawnTypeEnemy>(this._dropdownSpawnableEnemies);
            this.LoadDropdown<TrapSpawnType>(this._dropdownSpawnableTraps);
        }

        private void Update()
        {
            this._iconGamepad.SetActive(Gamepad.current != null);
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
            var spawnType = (SpawnTypeEnemy)this._dropdownSpawnableEnemies.value;
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
            var spawnType = (TrapSpawnType)this._dropdownSpawnableTraps.value;
            Transform spawn = SpawnablePool.SpawnTrap<Transform>(spawnType);
            spawn.position = this._debugUtils.GetRandomCircularPosition(Vector3.zero, 15f, 10f);

            spawn.gameObject.SetActive(true);
        }

        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.DEBUG, SceneID.HOME);
            SpawnablePool.DisableAll();
        }

        private void LoadDropdown<T>(TMP_Dropdown dropdown)
        {
            dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new();
            foreach (string spawnTypeName in Enum.GetNames(typeof(T)))
            {
                options.Add(new TMP_Dropdown.OptionData(spawnTypeName));
            }
            dropdown.AddOptions(options);
        }
    }
}