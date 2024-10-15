using Game.Database;
using System;
using Unity.AI.Navigation;
using UnityEngine;
using Utils;

namespace Game
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Components")]
        [SerializeField] private SpawnablePool _spawnablePool;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ToastHandler _toastHandler;
        [SerializeField] private NavMeshSurface _navMeshSurface;

        [Header("Database")]
        [SerializeField, ReadOnly] private PlayerConfigDatabase _playerConfig;
        [SerializeField, ReadOnly] private EnemyConfigDatabase _enemyConfig;
        [SerializeField, ReadOnly] private WeaponConfigDatabase _weaponConfig;
        [SerializeField, ReadOnly] private MapConfigDatabase _mapConfig;

        public IPlayerConfig PlayerConfig => this._playerConfig.Config;
        public IEnemyConfig[] EnemyConfig => this._enemyConfig.Config;
        public IWeaponConfig[] WeaponConfig => this._weaponConfig.Config;

        private void OnValidate()
        {
            if (this._playerConfig == null)
                this._playerConfig = Resources.Load<PlayerConfigDatabase>(ProjectPaths.PLAYER_CONFIG_DATABASE);

            if (this._enemyConfig == null)
                this._enemyConfig = Resources.Load<EnemyConfigDatabase>(ProjectPaths.ENEMY_CONFIG_DATABASE);

            if (this._weaponConfig == null)
                this._weaponConfig = Resources.Load<WeaponConfigDatabase>(ProjectPaths.WEAPON_CONFIG_DATABASE);

            if (this._mapConfig == null)
                this._mapConfig = Resources.Load<MapConfigDatabase>(ProjectPaths.MAP_CONFIG_DATABASE);
        }

        private void Awake()
        {
            this._spawnablePool.Init(this);
            this._playerController.Init(this);
        }

        public void OnGenerateMap()
        {
            new MapGenerator().Generate(this._mapConfig.Config, (room) =>
            {
                RoomGenerator roomGenerator = Instantiate(room.Prefab);
                roomGenerator.name = $"Room #{room.Id}";
                roomGenerator.transform.SetParent(base.transform);
                roomGenerator.Generate(room.SquaredSize, room.WallHeight, roomGenerator.transform);
                roomGenerator.transform.position = new Vector3(room.Position.x, 0, room.Position.y);
            },
            this._navMeshSurface.BuildNavMesh);
        }

        public void OnPlayerDeath()
        {
            this._toastHandler.Show("You died!");
        }

        public void OnPlayerRespawn()
        {
            this._toastHandler.Show("You Respawned!");
        }

        public void OnEnemyKill(IEnemyConfig enemy)
        {
            this._playerController.OnEnemyKill(enemy);
        }
    }
}
