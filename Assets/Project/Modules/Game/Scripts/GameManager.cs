using Game.Database;
using UnityEngine;
using Utils;

namespace Game
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [Header("Components")]
        [SerializeField] private SpawnablePool _spawnablePool;
        [SerializeField] private PlayerController _playerController;

        [Header("Database")]
        [SerializeField, ReadOnly] private CharacterConfigDatabase<CharacterConfig> _playerConfig;
        [SerializeField, ReadOnly] private EnemyConfigDatabase _enemyConfig;
        [SerializeField, ReadOnly] private WeaponConfigDatabase _weaponConfig;

        public ICharacterConfig PlayerConfig => this._playerConfig.Config;
        public IEnemyConfig[] EnemyConfig => this._enemyConfig.Config;
        public IWeaponConfig[] WeaponConfig => this._weaponConfig.Config;

        private void OnValidate()
        {
            if (this._playerConfig == null)
                this._playerConfig = Resources.Load<CharacterConfigDatabase<CharacterConfig>>(ProjectPaths.PLAYER_CONFIG_DATABASE);

            if (this._enemyConfig == null)
                this._enemyConfig = Resources.Load<EnemyConfigDatabase>(ProjectPaths.ENEMY_CONFIG_DATABASE);

            this._weaponConfig = Resources.Load<WeaponConfigDatabase>(ProjectPaths.WEAPON_CONFIG_DATABASE);
        }

        private void Awake()
        {
            this._spawnablePool.Init(this);
            this._playerController.Init(this);
        }
    }
}