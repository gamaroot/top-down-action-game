using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Utils;

namespace Game
{
    [RequireComponent(typeof(BoxCollider))]
    public class Room : MonoBehaviour, IRoom
    {
        public bool HasVisited { get; private set; }
        public bool IsPlayerHere { get; private set; }

        public int Id { get; private set; }

        private int _totalEnemies;
        private int _killedEnemies;

        private GameObject[] _doors;
        private GameObject _content;
        private List<GameObject> _waypoints;
        private Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> _spawnConfig;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.activeSelf &&
                other.CompareTag(Tags.Player.ToString()))
            {
                Debug.Log($"Player entered room #{this.Id}");
                this._content.SetActive(true);
                this.IsPlayerHere = true;

                if (this.HasVisited)
                    return;

                this.SpawnEnemies();
                this.SpawnTraps();

                this.HasVisited = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()))
            {
                this.IsPlayerHere = false;
                Debug.Log($"Player left room #{this.Id}");
                this.HideIfNotVisited();
            }
        }

        public Room Init(int id, GameObject[] doors, List<GameObject> waypoints, GameObject content,
                         Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> spawnConfig)
        {
            this.Id = id;
            this._doors = doors;

            this._content = content;
            this._waypoints = waypoints;
            this._spawnConfig = spawnConfig;

            return this;
        }

        public void OpenAllDoors()
        {
            for (int index = 0; index < this._doors.Length; index++)
            {
                this._doors[index].SetActive(false);
            }
        }

        public void ShowIfVisited()
        {
            if (this.HasVisited)
                this._content.SetActive(true);
        }

        public void HideIfPlayerIsNotHere()
        {
            if (!this.IsPlayerHere)
                this._content.SetActive(false);
        }

        private void HideIfNotVisited()
        {
            this._content.SetActive(false);
        }

        private void SpawnEnemies()
        {
            this._totalEnemies = this._spawnConfig.Item1.Length;
            for (int index = 0; index < this._totalEnemies; index++)
            {
                var config = this._spawnConfig.Item1[index];
                var spawn = SpawnablePool.SpawnEnemy<BehaviorGraphAgent>(config.Type);
                spawn.transform.SetParent(this._content.transform);
                spawn.transform.position = config.Position;
                spawn.gameObject.SetActive(true);
                spawn.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);

                var enemyHealthController = spawn.GetComponent<HealthController>();
                void OnEnemyKilled()
                {
                    enemyHealthController.OnDeathListener -= OnEnemyKilled;
                    if (++this._killedEnemies == this._totalEnemies)
                    {
                        this.OpenAllDoors();
                    }
                }
                enemyHealthController.OnDeathListener += OnEnemyKilled;
            }
        }

        

        private void SpawnTraps()
        {
            for (int index = 0; index < this._spawnConfig.Item2.Length; index++)
            {
                var config = this._spawnConfig.Item2[index];
                var spawn = SpawnablePool.SpawnTrap<Transform>(config.Type);
                spawn.SetParent(this._content.transform);
                spawn.position = config.Position;
            }
        }
    }
}
