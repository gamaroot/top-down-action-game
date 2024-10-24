using DG.Tweening.Core.Easing;
using Game.Database;
using ScreenNavigation;
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

        [Header("Database")]
        [SerializeField, ReadOnly] private PlayerConfigDatabase _playerConfig;
        [SerializeField, ReadOnly] private EnemyConfigDatabase _enemyConfig;
        [SerializeField, ReadOnly] private WeaponConfigDatabase _weaponConfig;
        [SerializeField, ReadOnly] private MapConfigDatabase _mapConfig;

        public IPlayerConfig PlayerConfig => this._playerConfig.Config;
        public IEnemyConfig[] EnemyConfig => this._enemyConfig.Config;
        public IWeaponConfig[] WeaponConfig => this._weaponConfig.Config;

        public GameState GameState => this._gameStateHandler.GameState;

        private GameStateHandler _gameStateHandler;
        private IRoom[] _rooms;

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

        private void Start()
        {
            this._gameStateHandler = new();
            this._spawnablePool.Init(this);

            SceneNavigator.Instance.AddListenerOnScreenStateChange(this.OnSceneChanged);
        }

        private void OnDestroy()
        {
            this.OnGameQuit();
        }

        public void OnPlayerStateUpdate(PlayerState playerState)
        {
            this._gameStateHandler.GameState.PlayerState = playerState;
        }

        public void OnPlayerReceivedXp(float xp)
        {
            this._gameStateHandler.GameState.PlayerState.XP += xp;
        }

        public void OnPlayerLevelUp()
        {
            this._gameStateHandler.GameState.PlayerState.Level++;
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

        public void OnMapVisibilityChange(bool visible)
        {
            for (int index = 0; index < this._rooms.Length; index++)
            {
                if (this._rooms == null) // Prevent null reference exception
                    break;

                if (visible)
                    this._rooms[index].ShowIfVisited();
                else
                    this._rooms[index].HideIfPlayerIsNotHere();
            }
        }

        private void OnGameStart()
        {
            // Randomize seed
            int seed = Random.Range(0, int.MaxValue);
            Random.InitState(seed);
            this._gameStateHandler.GameState.Seed = seed;

            // Generate map
            this._rooms = new MapGenerator().Generate(this._mapConfig.Config, base.transform);

            this._playerController.Activate(true, this);
        }

        private void OnGameQuit()
        {
            this._gameStateHandler.Save();
            Statistics.Instance.OnGameQuit();

            this._rooms = null;
            this._playerController.Activate(false, this);
        }

        private void OnSceneChanged(SceneID sceneID, SceneState sceneState)
        {
            switch (sceneID)
            {
                case SceneID.GAME:
                case SceneID.DEBUG:
                    if (sceneState == SceneState.LOADING)
                        this.OnGameStart();
                    else if (sceneState == SceneState.UNLOADED)
                        this.OnGameQuit();
                    break;
            }
        }
    }
}
