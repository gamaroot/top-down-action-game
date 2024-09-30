using ScreenNavigation;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Utils;
using Unity.Behavior;

namespace Game
{
    public class DebugScreenEvents : MonoBehaviour
    {
        [SerializeField, ReadOnly] private GameObject _player;
        [SerializeField] private TMP_Dropdown _dropdownSpawnable;
        [SerializeField] private List<GameObject> _waypoints;

        private readonly DebugUtils _debugUtils = new();

        private void OnValidate()
        {
            if (this._player == null)
                this._player = GameObject.FindWithTag(Tags.Player.ToString());

            if (this._dropdownSpawnable != null)
            {
                this._dropdownSpawnable.ClearOptions();

                List<TMP_Dropdown.OptionData> options = new();
                foreach (string spawnTypeName in Enum.GetNames(typeof(SpawnType)))
                {
                    options.Add(new TMP_Dropdown.OptionData(spawnTypeName));
                }
                this._dropdownSpawnable.AddOptions(options);
            }
        }

        public void OnSpawnButtonClick()
        {
            var spawnType = (SpawnType)this._dropdownSpawnable.value;
            Transform spawn = SpawnablePool.Spawn<Transform>(spawnType);
            spawn.position = this._debugUtils.GetRandomCircularPosition(this._player.transform.position, 10f, 5f);

            if (spawn.tag == Tags.Enemy.ToString())
            {
                BehaviorGraphAgent enemy = spawn.GetComponent<BehaviorGraphAgent>();
                enemy.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);
            }

            spawn.gameObject.SetActive(true);
        }

        public void OnQuitButtonClick()
        {
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.DEBUG, SceneID.HOME);
        }
    }
}