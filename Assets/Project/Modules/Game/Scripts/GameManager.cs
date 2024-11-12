using Game.Database;
using ScreenNavigation;
using System;
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
        [SerializeField] private VFX _vfx;

        [Header("Database")]
        [SerializeField, ReadOnly] private PlayerConfigDatabase _playerConfig;
        [SerializeField, ReadOnly] private EnemyConfigDatabase _enemyConfig;
        [SerializeField, ReadOnly] private WeaponConfigDatabase _weaponConfig;
        [SerializeField, ReadOnly] private MapConfigDatabase _mapConfig;

        public Action<int, int> OnPlayerHealthUpdateListener { get; set; }
        public Action<float, float> OnPlayerXpUpdateListener { get; set; }
        public Action<int> OnPlayerLevelUpdateListener { get; set; }

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

        public void OnPlayerStateUpdated(PlayerState playerState)
        {
            this._gameStateHandler.GameState.PlayerState = playerState;
        }

        public void OnPlayerHealthUpdated(int currenHealth, int maxHealth)
        {
            this._gameStateHandler.GameState.PlayerState.CurrentHealth = currenHealth;
            this.OnPlayerHealthUpdateListener.Invoke(currenHealth, maxHealth);
        }

        public void OnPlayerRecoverHealth(float healthPercentage)
        {
            this._vfx.ShowHealLayer(healthPercentage);
        }

        public void OnPlayerLoseHealth(float healthPercentage)
        {
            this._vfx.ShowDamageLayer(healthPercentage);
        }

        public void OnPlayerXpUpdated(float xp, float xpToNextLevel)
        {
            Debug.Log($"Updating XP {xp} / {xpToNextLevel}");
            this._gameStateHandler.GameState.PlayerState.XP = xp;

            this.OnPlayerXpUpdateListener.Invoke(xp, xpToNextLevel);
        }

        public void OnPlayerLevelUp(float nextLevelXp)
        {
            this._gameStateHandler.GameState.PlayerState.XP = 0;
            int newLevel = ++this._gameStateHandler.GameState.PlayerState.Level;

            this.OnPlayerXpUpdateListener.Invoke(0, nextLevelXp);
            this.OnPlayerLevelUpdateListener.Invoke(newLevel);
        }

        public void OnPlayerDeath()
        {
            Statistics.Instance.OnPlayerDeath();
            Statistics.Instance.OnGameOver(this._gameStateHandler.GameState.PlayerState.XpGained);

            int previousLevel = this._gameStateHandler.GameState.PlayerState.InitialLevel;
            int currentLevel = this._gameStateHandler.GameState.PlayerState.Level;

            if (SceneNavigator.Instance.IsSceneOpened(SceneID.DEBUG))
                return;

            SceneNavigator.Instance.SetSceneParams(SceneID.GAME_OVER, (previousLevel, currentLevel, this._rooms));
            SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.GAME, SceneID.GAME_OVER);
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
            int seed = UnityEngine.Random.Range(0, int.MaxValue);
            UnityEngine.Random.InitState(seed);
            this._gameStateHandler.GameState.Seed = seed;

            // Generate map
            this._rooms = new MapGenerator().Generate(this._mapConfig.Config, base.transform);
            
            this._gameStateHandler.OnGameStart();
            Statistics.Instance.OnGameStart();
        }

        private void OnGameQuit()
        {
            this._gameStateHandler.Save();
            if (!this._playerController.IsDead)
            {
                Statistics.Instance.OnGameOver(this._gameStateHandler.GameState.PlayerState.XpGained);
            }
            SpawnablePool.DisableAll();

            this._rooms = null;
            for (int index = 0; index < base.transform.childCount; index++)
            {
                Destroy(base.transform.GetChild(index).gameObject);
            }
        }

        private void OnSceneChanged(SceneID sceneID, SceneState sceneState)
        {
            switch (sceneID)
            {
                case SceneID.GAME:
                    switch (sceneState)
                    {
                        case SceneState.LOADING:
                            this.OnGameStart();
                            break;
                        case SceneState.ANIMATING_SHOW:
                            this.ActivatePlayerControl();
                            break;
                        case SceneState.UNLOADED:
                            this.OnGameQuit();
                            this.DeactivatePlayerControl();

                            this._vfx.DisableAll();
                            break;
                    }
                    break;
                case SceneID.DEBUG:
                    switch (sceneState)
                    {
                        case SceneState.ANIMATING_SHOW:
                            this.ActivatePlayerControl();
                            break;
                        case SceneState.UNLOADED:
                            this.DeactivatePlayerControl();
                            break;
                    }
                    break;
            }
        }

        private void ActivatePlayerControl()
        {
            this._playerController.ResetToDefaultPosition();
            this._playerController.Activate(true, this);
        }

        private void DeactivatePlayerControl()
        {
            this._playerController.Activate(false, this);
        }
    }
}
