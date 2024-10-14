using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace Game.Database
{
    public class GameSetupEditor : EditorWindow
    {
        private int _tabIndex;
        private readonly string[] _tabs = { "Player", "Enemy", "Weapon", "Map" };

        private PlayerConfig _playerConfig = new();
        private MapConfig _mapConfig = new();
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
                case 3: this.DrawMapTab(); break;
            }

            if (GUILayout.Button("Save"))
                this.SaveData();
        }

        private void DrawPlayerTab()
        {
            // XP
            this._playerConfig.StatsPointsPerLevel = EditorGUILayout.IntField("Stats Points per Level", this._playerConfig.StatsPointsPerLevel);
            this._playerConfig.XpToNextLevel = EditorGUILayout.FloatField("XP to Next Level", this._playerConfig.XpToNextLevel);
            this._playerConfig.XpToNextLevelFactor = EditorGUILayout.FloatField("XP to Next Level Factor", this._playerConfig.XpToNextLevelFactor);

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
                if (this._enemyConfigs[index] == null)
                    this._enemyConfigs[index] = new();

                this._enemyConfigs[index].Type = (SpawnTypeEnemy)index;
                this._enemyFoldouts[index] = EditorGUILayout.Foldout(this._enemyFoldouts[index], $"Enemy: {this._enemyConfigs[index].Type}", true);

                if (!this._enemyFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                // Reward
                this._enemyConfigs[index].XpReward = EditorGUILayout.FloatField("XP Reward", this._enemyConfigs[index].XpReward);

                // Health
                this._enemyConfigs[index].MaxHealth = EditorGUILayout.FloatField("Max Health", this._enemyConfigs[index].MaxHealth);
                this._enemyConfigs[index].DeathVFX = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Death VFX", this._enemyConfigs[index].DeathVFX);
                this._enemyConfigs[index].DeathSFX = (SFXTypeExplosion)EditorGUILayout.EnumPopup("Death SFX", this._enemyConfigs[index].DeathSFX);

                // Sensor
                this._enemyConfigs[index].TargetTag = (Tags)EditorGUILayout.EnumPopup("Target Tag", this._enemyConfigs[index].TargetTag);

                int layer = EditorGUILayout.MaskField("Obstacle Layer", this._enemyConfigs[index].ObstacleLayer.value, UnityEditorInternal.InternalEditorUtility.layers);
                this._enemyConfigs[index].ObstacleLayer = (LayerMask)layer;

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
            for (int index = 0; index < this._weaponConfigs.Length; index++)
            {
                if (this._weaponConfigs[index] == null)
                    this._weaponConfigs[index] = new();

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

        private void DrawMapTab()
        {
            this._mapConfig.MinRooms = EditorGUILayout.IntField("Min Rooms", this._mapConfig.MinRooms);
            this._mapConfig.MaxRooms = EditorGUILayout.IntField("Max Rooms", this._mapConfig.MaxRooms);

            EditorGUILayout.LabelField("Room Prefabs", EditorStyles.boldLabel);

            var indicesToRemove = new List<int>(); // List to keep track of indices to remove

            // Ensures there is at least one element in the list
            if (this._mapConfig.RoomPrefabs.Count == 0)
                this._mapConfig.RoomPrefabs.Add(null);

            // Create a new list to hold the Room Prefabs
            for (int index = 0; index < this._mapConfig.RoomPrefabs.Count; index++)
            {
                EditorGUILayout.BeginHorizontal();

                // Draw a field for each prefab in the list
                this._mapConfig.RoomPrefabs[index] = (GameObject)EditorGUILayout.ObjectField($"Room #{index + 1}", this._mapConfig.RoomPrefabs[index], typeof(GameObject), false);

                // Add "+" button next to the prefab field
                if (GUILayout.Button("+", GUILayout.Width(25)))
                {
                    this._mapConfig.RoomPrefabs.Add(null); // Add a new prefab
                }

                // Add "-" button next to the prefab field
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    indicesToRemove.Add(index); // Mark index for removal
                }

                EditorGUILayout.EndHorizontal();
            }

            // Remove marked prefabs after the loop
            foreach (int index in indicesToRemove.OrderByDescending(i => i)) // Remove from the end to avoid index shifting
            {
                this._mapConfig.RoomPrefabs.RemoveAt(index);
            }
        }

        private void LoadData()
        {
            this._enemyConfigs = new EnemyConfig[Enum.GetValues(typeof(SpawnTypeEnemy)).Length];
            this._weaponConfigs = new WeaponConfig[Enum.GetValues(typeof(WeaponType)).Length];

            this._enemyFoldouts = new bool[this._enemyConfigs.Length];
            this._weaponFoldouts = new bool[this._weaponConfigs.Length];

            this.LoadData(ProjectPaths.PLAYER_CONFIG_DATABASE, ref this._playerConfig);
            this.LoadData(ProjectPaths.ENEMY_CONFIG_DATABASE, ref this._enemyConfigs);
            this.LoadData(ProjectPaths.WEAPON_CONFIG_DATABASE, ref this._weaponConfigs);
            this.LoadData(ProjectPaths.MAP_CONFIG_DATABASE, ref this._mapConfig);
        }

        private void LoadData<T>(string path, ref T configField)
        {
            configField = this.LoadData<T>(path);
        }

        private void LoadData<T>(string path, ref T[] configField)
        {
            var data = this.LoadData<T[]>(path);
            configField = data.Length > 0 ? data : configField;
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
            this.SaveData(this._mapConfig, typeof(MapConfigDatabase), ProjectPaths.MAP_CONFIG_DATABASE);

            AssetDatabase.SaveAssets();
        }

        private void SaveData<T>(T configData, Type databaseType, string databasePath) where T : class
        {
            var configDatabase = ScriptableObject.CreateInstance(databaseType) as GameConfigDatabase<T>;
            configDatabase.SetData(configData);
            AssetDatabase.CreateAsset(configDatabase, string.Format(ProjectPaths.DATABASE_PATH, databasePath));
            AssetDatabase.Refresh();
        }
    }
}