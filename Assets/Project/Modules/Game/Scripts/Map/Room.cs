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
        [SerializeField, ReadOnly] private BoxCollider _collider;
        [SerializeField] private GameObject _spawnerParent;

        public bool HasVisited { get; private set; }
        public bool IsPlayerHere { get; private set; }

        public int Id { get; private set; }
        public bool[] Neighbors { get; private set; }

        private GameObject _content;
        private List<GameObject> _waypoints;
        private Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> _spawnConfig;

        private void OnValidate()
        {
            if (this._collider == null)
                this._collider = base.GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.activeSelf &&
                other.CompareTag(Tags.Player.ToString()))
            {
                Debug.Log($"Player entered room #{this.Id}");
                this._content.SetActive(true);
                this.HasVisited = true;
                this.IsPlayerHere = true;

                this.SpawnEnemies();
                this.SpawnTraps();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()))
            {
                this.IsPlayerHere = false;
                Debug.Log($"Player left room #{this.Id}");
                this.Hide();
            }
        }

        public void Init(int id, float squaredSize, bool[] neighbors, List<GameObject> waypoints, GameObject content,
                         Tuple<SpawnConfig<SpawnTypeEnemy>[], SpawnConfig<SpawnTypeTrap>[]> spawnConfig)
        {
            this.Id = id;
            this.Neighbors = neighbors;

            this._content = content;
            this._waypoints = waypoints;
            this._spawnConfig = spawnConfig;

            this._collider.size = new Vector3(squaredSize, 1f, squaredSize);
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

        private void Hide()
        {
            this._content.SetActive(false);
        }

        private void SpawnEnemies()
        {
            for (int index = 0; index < this._spawnConfig.Item1.Length; index++)
            {
                var config = this._spawnConfig.Item1[index];
                var spawn = SpawnablePool.SpawnEnemy<BehaviorGraphAgent>(config.Type);
                spawn.transform.SetParent(this._content.transform);
                spawn.transform.position = config.Position;
                spawn.gameObject.SetActive(true);
                spawn.BlackboardReference.SetVariableValue("Waypoints", this._waypoints);
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
