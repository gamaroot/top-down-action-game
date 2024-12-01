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
        public RoomType Type { get; private set; }
        public Vector3 Position => base.transform.position;
        public float Size => base.transform.localScale.x;

        private int _totalEnemies;
        private int _killedEnemies;

        private GameObject[] _doors;
        private List<GameObject> _content;
        private List<GameObject> _waypoints;
        private Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> _spawnConfig;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.activeSelf &&
                other.CompareTag(Tags.Player.ToString()))
            {
                Debug.Log($"Player entered room #{this.Id}");
                this.SetContentVisible(true);
                this.IsPlayerHere = true;

                if (this.HasVisited)
                    return;

                if (this.Type == RoomType.BOSS)
                    BGM.PlayMusic(BGMType.BOSS);

                this.SpawnEnemies();
                this.SpawnTraps();

                this.HasVisited = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()))
            {
                if (this.Type == RoomType.BOSS)
                    BGM.PlayMusic(BGMType.GAMEPLAY);

                this.IsPlayerHere = false;
                Debug.Log($"Player left room #{this.Id}");
                this.HideIfNotVisited();
            }
        }

        public Room Init(int id, RoomType type, GameObject[] doors, List<GameObject> waypoints, GameObject content,
                         Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> spawnConfig)
        {
            this._content = new List<GameObject> { content };

            this.Id = id;
            this.Type = type;
            this._doors = doors;

            this._waypoints = waypoints;
            this._spawnConfig = spawnConfig;

            return this;
        }

        public void OpenAllDoors()
        {
            for (int index = 0; index < this._doors.Length; index++)
            {
                GameObject door = this._doors[index];
                if (door)
                    door.SetActive(false);
            }
        }

        public void ShowIfVisited()
        {
            if (this.HasVisited)
                this.SetContentVisible(true);
        }

        public void HideIfPlayerIsNotHere()
        {
            if (!this.IsPlayerHere)
                this.SetContentVisible(false);
        }

        private void HideIfNotVisited()
        {
            this.SetContentVisible(false);
        }

        private void SpawnEnemies()
        {
            this._totalEnemies = this._spawnConfig.Item1.Length;
            for (int index = 0; index < this._totalEnemies; index++)
            {
                SpawnConfig<SpawnTypeEnemy> config = this._spawnConfig.Item1[index];

                BehaviorGraphAgent spawn = SpawnablePool.SpawnEnemy(config.Type);
                spawn.transform.position = config.Position;
                spawn.gameObject.SetActive(true);
                spawn.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);

                var enemyHealthController = spawn.GetComponent<HealthController>();
                void OnEnemyKilled()
                {
                    enemyHealthController.Listener.OnDeath -= OnEnemyKilled;
                    if (++this._killedEnemies == this._totalEnemies)
                    {
                        this.OpenAllDoors();
                    }
                    this._content.Remove(spawn.gameObject);
                }
                enemyHealthController.Listener.OnDeath += OnEnemyKilled;

                this._content.Add(spawn.gameObject);
            }
        }

        

        private void SpawnTraps()
        {
            for (int index = 0; index < this._spawnConfig.Item2.Length; index++)
            {
                SpawnConfig<SpawnTypeTrap> config = this._spawnConfig.Item2[index];

                GameObject spawn = SpawnablePool.SpawnTrap(config.Type);
                spawn.transform.position = config.Position;
                spawn.SetActive(true);

                this._content.Add(spawn.gameObject);
            }
        }

        private void SetContentVisible(bool isVisible)
        {
            this._content.ForEach(obj => obj.SetActive(isVisible));
        }
    }
}
