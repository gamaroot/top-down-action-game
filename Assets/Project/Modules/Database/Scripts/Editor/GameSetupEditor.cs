using System;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Game.Database
{
    public class GameSetupEditor : EditorWindow
    {
        private int _tabIndex;
        private readonly string[] _tabs = { "Player", "Enemy", "Weapon" };

        private CharacterConfig _playerConfig = new();
        private EnemyConfig[] _enemyConfigs;
        private WeaponConfig[] _weaponConfigs;

        private bool[] _enemyFoldouts;
        private bool[] _weaponFoldouts;

        [MenuItem("Tools/Game Setup")]
        public static void ShowWindow()
        {
            GetWindow<GameSetupEditor>("Game Setup");
        }

        private void OnEnable()
        {
            this.LoadData();
        }

        private void OnGUI()
        {
            this._tabIndex = GUILayout.Toolbar(this._tabIndex, this._tabs);

            switch (this._tabIndex)
            {
                case 0: this.DrawPlayerTab(); break;
                case 1: this.DrawEnemyTab(); break;
                case 2: this.DrawWeaponTab(); break;
            }

            if (GUILayout.Button("Save"))
                this.SaveData();
        }

        private void DrawPlayerTab()
        {
            // Health
            this._playerConfig.MaxHealth = EditorGUILayout.FloatField("Max Health", this._playerConfig.MaxHealth);
            this._playerConfig.DeathVFX = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Death VFX", this._playerConfig.DeathVFX);
            this._playerConfig.DeathSFX = (SFXTypeExplosion)EditorGUILayout.EnumPopup("Death SFX", this._playerConfig.DeathSFX);

            // Movement
            this._playerConfig.MovementSpeed = EditorGUILayout.FloatField("Movement Speed", this._playerConfig.MovementSpeed);
            this._playerConfig.DashSpeed = EditorGUILayout.FloatField("Dash Speed", this._playerConfig.DashSpeed);
            this._playerConfig.DashDuration = EditorGUILayout.FloatField("Dash Duration", this._playerConfig.DashDuration);
            this._playerConfig.DashCooldown = EditorGUILayout.FloatField("Dash Cooldown", this._playerConfig.DashCooldown);

            // Parry
            this._playerConfig.ParryDuration = EditorGUILayout.FloatField("Parry Duration", this._playerConfig.ParryDuration);
            this._playerConfig.ParryCooldown = EditorGUILayout.FloatField("Parry Cooldown", this._playerConfig.ParryCooldown);
        }

        private void DrawEnemyTab()
        {
            for (int index = 0; index < this._enemyConfigs.Length; index++)
            {
                this._enemyConfigs[index].Type = (SpawnTypeEnemy)index;
                this._enemyFoldouts[index] = EditorGUILayout.Foldout(this._enemyFoldouts[index], $"Enemy: {this._enemyConfigs[index].Type}", true);

                if (!this._enemyFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                // Health
                this._enemyConfigs[index].MaxHealth = EditorGUILayout.FloatField("Max Health", this._enemyConfigs[index].MaxHealth);
                this._enemyConfigs[index].DeathVFX = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Death VFX", this._enemyConfigs[index].DeathVFX);
                this._enemyConfigs[index].DeathSFX = (SFXTypeExplosion)EditorGUILayout.EnumPopup("Death SFX", this._enemyConfigs[index].DeathSFX);

                // Sensor
                this._enemyConfigs[index].TargetTag = (Tags)EditorGUILayout.EnumPopup("Target Tag", this._enemyConfigs[index].TargetTag);
                this._enemyConfigs[index].ObstacleLayer = EditorGUILayout.LayerField("Obstacle Layer", this._enemyConfigs[index].ObstacleLayer);
                this._enemyConfigs[index].DetectionRadius = EditorGUILayout.FloatField("Detection Radius", this._enemyConfigs[index].DetectionRadius);

                // Movement
                this._enemyConfigs[index].MovementSpeed = EditorGUILayout.FloatField("Movement Speed", this._enemyConfigs[index].MovementSpeed);
                this._enemyConfigs[index].DashSpeed = EditorGUILayout.FloatField("Dash Speed", this._enemyConfigs[index].DashSpeed);
                this._enemyConfigs[index].DashPossibility = EditorGUILayout.FloatField("Dash Possibility", this._enemyConfigs[index].DashPossibility);
                this._enemyConfigs[index].DashDuration = EditorGUILayout.FloatField("Dash Duration", this._enemyConfigs[index].DashDuration);
                this._enemyConfigs[index].DashCooldown = EditorGUILayout.FloatField("Dash Cooldown", _enemyConfigs[index].DashCooldown);

                // Parry
                this._enemyConfigs[index].ParryPossibility = EditorGUILayout.FloatField("Parry Possibility", this._enemyConfigs[index].ParryPossibility);
                this._enemyConfigs[index].ParryCooldown = EditorGUILayout.FloatField("Parry Cooldown", this._enemyConfigs[index].ParryCooldown);

                EditorGUI.indentLevel--;
            }
        }

        private void DrawWeaponTab()
        {
            for (int index = 0; index < this._enemyConfigs.Length; index++)
            {
                this._weaponConfigs[index].Type = (WeaponType)index;
                this._weaponFoldouts[index] = EditorGUILayout.Foldout(this._weaponFoldouts[index], $"Weapon: {this._weaponConfigs[index].Type}", true);

                if (!this._weaponFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                this._weaponConfigs[index].Damage = EditorGUILayout.FloatField("Damage", this._weaponConfigs[index].Damage);
                this._weaponConfigs[index].ShootInterval = EditorGUILayout.FloatField("Shoot Interval", this._weaponConfigs[index].ShootInterval);
                this._weaponConfigs[index].Range = EditorGUILayout.FloatField("Range", this._weaponConfigs[index].Range);
                this._weaponConfigs[index].SfxOnShoot = (SFXTypeProjectile)EditorGUILayout.EnumPopup("SFX on Shoot", this._weaponConfigs[index].SfxOnShoot);
                this._weaponConfigs[index].SfxOnExplode = (SFXTypeExplosion)EditorGUILayout.EnumPopup("SFX on Explode", this._weaponConfigs[index].SfxOnExplode);
                this._weaponConfigs[index].ExplosionType = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Explosion Type", this._weaponConfigs[index].ExplosionType);
                this._weaponConfigs[index].ProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", this._weaponConfigs[index].ProjectileSpeed);
                this._weaponConfigs[index].ChanceOfBeingPinky = EditorGUILayout.FloatField("Chance of being Pinky", this._weaponConfigs[index].ChanceOfBeingPinky);

                EditorGUI.indentLevel--;
            }
        }

        private void LoadData()
        {
            this._enemyConfigs = new EnemyConfig[Enum.GetValues(typeof(SpawnTypeEnemy)).Length];
            this._weaponConfigs = new WeaponConfig[Enum.GetValues(typeof(WeaponType)).Length];

            this._enemyFoldouts = new bool[this._enemyConfigs.Length];
            this._weaponFoldouts = new bool[this._weaponConfigs.Length];

            this._playerConfig = this.LoadData<CharacterConfig>(ProjectPaths.PLAYER_CONFIG_DATABASE);
            this._enemyConfigs = this.LoadData<EnemyConfig[]>(ProjectPaths.ENEMY_CONFIG_DATABASE);
            this._weaponConfigs = this.LoadData<WeaponConfig[]>(ProjectPaths.WEAPON_CONFIG_DATABASE);
        }

        private T LoadData<T>(string filename)
        {
            return Resources.Load<GameConfigDatabase<T>>(filename).Config;
        }

        private void SaveData()
        {
            this.SaveData(this._playerConfig, typeof(PlayerConfigDatabase), ProjectPaths.PLAYER_CONFIG_DATABASE);
            this.SaveData(this._enemyConfigs, typeof(EnemyConfigDatabase), ProjectPaths.ENEMY_CONFIG_DATABASE);
            this.SaveData(this._weaponConfigs, typeof(WeaponConfigDatabase), ProjectPaths.WEAPON_CONFIG_DATABASE);

            AssetDatabase.SaveAssets();
        }

        private void SaveData<T>(T configData, Type databaseType, string databasePath) where T : class
        {
            var configDatabase = ScriptableObject.CreateInstance(databaseType) as GameConfigDatabase<T>;
            configDatabase.SetData(configData);
            AssetDatabase.CreateAsset(configDatabase, string.Format(ProjectPaths.DATABASE_PATH, databasePath));
        }
    }
}