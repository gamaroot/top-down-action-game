using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
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
        private bool[] _roomFoldouts;
        private bool _enemySpawnFoldout;
        private bool _trapSpawnFoldout;

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

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            this.DrawSaveButton();
        }

        private void DrawSaveButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            var style = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16
            };

            if (GUILayout.Button("Save", style, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.8f), GUILayout.Height(30f)))
            {
                this.SaveData();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPlayerTab()
        {
            var newStats = this._playerConfig.Stats;

            EditorGUILayout.LabelField("[Basic]", EditorStyles.boldLabel);
            newStats.MaxHealth = EditorGUILayout.IntField("Max Health", newStats.MaxHealth);
            newStats.MovementSpeed = EditorGUILayout.FloatField("Movement Speed", newStats.MovementSpeed);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Dash]", EditorStyles.boldLabel);
            newStats.DashSpeed = EditorGUILayout.FloatField("Dash Speed", newStats.DashSpeed);
            newStats.DashDuration = EditorGUILayout.FloatField("Dash Duration", newStats.DashDuration);
            newStats.DashCooldown = EditorGUILayout.FloatField("Dash Cooldown", newStats.DashCooldown);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Parry]", EditorStyles.boldLabel);
            newStats.ParryDuration = EditorGUILayout.FloatField("Parry Duration", newStats.ParryDuration);
            newStats.ParryCooldown = EditorGUILayout.FloatField("Parry Cooldown", newStats.ParryCooldown);

            this._playerConfig.Stats = newStats;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Level]", EditorStyles.boldLabel);
            this._playerConfig.BaseLevelUpXp = EditorGUILayout.FloatField("XP to Next Level", this._playerConfig.BaseLevelUpXp);
            this._playerConfig.XpToNextLevelFactor = EditorGUILayout.FloatField("XP to Next Level Factor", this._playerConfig.XpToNextLevelFactor);
            this._playerConfig.InitialStatsPoints = EditorGUILayout.IntField("Initial Stats Points", this._playerConfig.InitialStatsPoints);
            this._playerConfig.StatsPointsPerLevel = EditorGUILayout.IntField("Stats Points per Level", this._playerConfig.StatsPointsPerLevel);

            CharacterStats newExtraStatsPerPoint = this._playerConfig.ExtraStatsPerPoint;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Stats per Point]", EditorStyles.boldLabel);
            newExtraStatsPerPoint.MaxHealth = EditorGUILayout.IntField("Max Health", newExtraStatsPerPoint.MaxHealth);
            newExtraStatsPerPoint.MovementSpeed = EditorGUILayout.FloatField("Movement Speed", newExtraStatsPerPoint.MovementSpeed);
            newExtraStatsPerPoint.DashSpeed = EditorGUILayout.FloatField("Dash Speed", newExtraStatsPerPoint.DashSpeed);
            newExtraStatsPerPoint.DashDuration = EditorGUILayout.FloatField("Dash Duration", newExtraStatsPerPoint.DashDuration);
            newExtraStatsPerPoint.DashCooldown = EditorGUILayout.FloatField("Dash Cooldown", newExtraStatsPerPoint.DashCooldown);
            newExtraStatsPerPoint.ParryDuration = EditorGUILayout.FloatField("Parry Duration", newExtraStatsPerPoint.ParryDuration);
            newExtraStatsPerPoint.ParryCooldown = EditorGUILayout.FloatField("Parry Cooldown", newExtraStatsPerPoint.ParryCooldown);

            this._playerConfig.ExtraStatsPerPoint = newExtraStatsPerPoint;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Limit Stats Points]", EditorStyles.boldLabel);
            this._playerConfig.MaxPointsForStats[0] = EditorGUILayout.IntField("Max Health", this._playerConfig.MaxPointsForStats[0]);
            this._playerConfig.MaxPointsForStats[1] = EditorGUILayout.IntField("Movement Speed", this._playerConfig.MaxPointsForStats[1]);
            this._playerConfig.MaxPointsForStats[2] = EditorGUILayout.IntField("Dash Speed", this._playerConfig.MaxPointsForStats[2]);
            this._playerConfig.MaxPointsForStats[3] = EditorGUILayout.IntField("Dash Duration", this._playerConfig.MaxPointsForStats[3]);
            this._playerConfig.MaxPointsForStats[4] = EditorGUILayout.IntField("Dash Cooldown", this._playerConfig.MaxPointsForStats[4]);
            this._playerConfig.MaxPointsForStats[5] = EditorGUILayout.IntField("Parry Duration", this._playerConfig.MaxPointsForStats[5]);
            this._playerConfig.MaxPointsForStats[6] = EditorGUILayout.IntField("Parry Cooldown", this._playerConfig.MaxPointsForStats[6]);


            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Others]", EditorStyles.boldLabel);
            this._playerConfig.DeathVFX = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Death VFX", this._playerConfig.DeathVFX);
            this._playerConfig.DeathSFX = (SFXTypeExplosion)EditorGUILayout.EnumPopup("Death SFX", this._playerConfig.DeathSFX);
        }

        private void DrawEnemyTab()
        {
            for (int index = 0; index < this._enemyConfigs.Length; index++)
            {
                if (this._enemyConfigs[index] == null) this._enemyConfigs[index] = new();
                this._enemyFoldouts[index] = EditorGUILayout.Foldout(this._enemyFoldouts[index], $"Enemy: {this._enemyConfigs[index].Type}", true);

                if (!this._enemyFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                CharacterStats newStats = this._enemyConfigs[index].Stats;

                EditorGUILayout.LabelField("[Basic]", EditorStyles.boldLabel);
                this._enemyConfigs[index].XpReward = EditorGUILayout.FloatField("XP Reward", this._enemyConfigs[index].XpReward);
                newStats.MaxHealth = EditorGUILayout.IntField("Max Health", this._enemyConfigs[index].Stats.MaxHealth);
                newStats.MovementSpeed = EditorGUILayout.FloatField("Movement Speed", this._enemyConfigs[index].Stats.MovementSpeed);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("[Sensor]", EditorStyles.boldLabel);
                this._enemyConfigs[index].TargetTag = (Tags)EditorGUILayout.EnumPopup("Target Tag", this._enemyConfigs[index].TargetTag);
                int layer = EditorGUILayout.MaskField("Obstacle Layer", this._enemyConfigs[index].ObstacleLayer.value, UnityEditorInternal.InternalEditorUtility.layers);
                this._enemyConfigs[index].ObstacleLayer = (LayerMask)layer;
                this._enemyConfigs[index].DetectionRadius = EditorGUILayout.FloatField("Detection Radius", this._enemyConfigs[index].DetectionRadius);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("[Others]", EditorStyles.boldLabel);
                this._enemyConfigs[index].DeathVFX = (SpawnTypeExplosion)EditorGUILayout.EnumPopup("Death VFX", this._enemyConfigs[index].DeathVFX);
                this._enemyConfigs[index].DeathSFX = (SFXTypeExplosion)EditorGUILayout.EnumPopup("Death SFX", this._enemyConfigs[index].DeathSFX);

                this._enemyConfigs[index].Stats = newStats;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        private void DrawWeaponTab()
        {
            for (int index = 0; index < this._weaponConfigs.Length; index++)
            {
                if (this._weaponConfigs[index] == null)
                    this._weaponConfigs[index] = new();

                this._weaponFoldouts[index] = EditorGUILayout.Foldout(this._weaponFoldouts[index], $"Weapon: {this._weaponConfigs[index].Type}", true);

                if (!this._weaponFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                this._weaponConfigs[index].Damage = EditorGUILayout.IntField("Damage", this._weaponConfigs[index].Damage);
                this._weaponConfigs[index].ShootInterval = EditorGUILayout.FloatField("Shoot Interval", this._weaponConfigs[index].ShootInterval);
                this._weaponConfigs[index].Range = EditorGUILayout.FloatField("Range", this._weaponConfigs[index].Range);
                this._weaponConfigs[index].ProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", this._weaponConfigs[index].ProjectileSpeed);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        private void DrawMapTab()
        {
            this._mapConfig.MinRooms = EditorGUILayout.IntField("Min Rooms", this._mapConfig.MinRooms);
            this._mapConfig.MaxRooms = EditorGUILayout.IntField("Max Rooms", this._mapConfig.MaxRooms);
            this._mapConfig.RoomSquaredSize = EditorGUILayout.FloatField("Room Squared Size", this._mapConfig.RoomSquaredSize);
            this._mapConfig.RoomWallHeight = EditorGUILayout.FloatField("Room Wall Height", this._mapConfig.RoomWallHeight);

            var indicesToRemove = new List<int>();

            if (this._mapConfig.RoomConfigs.Count == 0)
                this._mapConfig.RoomConfigs.Add(new());

            if (this._roomFoldouts == null || this._roomFoldouts.Length != this._mapConfig.RoomConfigs.Count)
                this._roomFoldouts = new bool[this._mapConfig.RoomConfigs.Count];

            int count = this._mapConfig.RoomConfigs.Count;
            for (int index = 0; index < count; index++)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                this._roomFoldouts[index] = EditorGUILayout.Foldout(this._roomFoldouts[index], $"[Room #{index + 1}]", true, EditorStyles.boldLabel);

                if (GUILayout.Button("-", GUILayout.Width(30)))
                    indicesToRemove.Add(this._mapConfig.RoomConfigs.Count - 1);

                if (GUILayout.Button("+", GUILayout.Width(30)))
                    this._mapConfig.RoomConfigs.Add(new());

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (!this._roomFoldouts[index])
                    continue;

                EditorGUI.indentLevel++;

                this._mapConfig.RoomConfigs[index].Prefab = (RoomGenerator)EditorGUILayout.ObjectField("Prefab", this._mapConfig.RoomConfigs[index].Prefab, typeof(RoomGenerator), false);

                if (this._mapConfig.RoomConfigs[index].Prefab.Type != RoomType.BASIC)
                    this._mapConfig.RoomConfigs[index].MinRoomsBefore = EditorGUILayout.IntField("Min Rooms Before", this._mapConfig.RoomConfigs[index].MinRoomsBefore);


                if (this._mapConfig.RoomConfigs[index].Prefab.Type != RoomType.BOSS)
                {
                    this._mapConfig.RoomConfigs[index].IsUnique = EditorGUILayout.Toggle("Is Unique", this._mapConfig.RoomConfigs[index].IsUnique);

                    this.DrawEnemySpawnProperties(index);
                    this.DrawTrapSpawnProperties(index);
                }
                else
                {
                    this._mapConfig.RoomConfigs[index].IsUnique = true;
                    EditorGUILayout.LabelField("Is Unique", "True");
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Enemy", GUILayout.Width(135f));

                    if (this._mapConfig.RoomConfigs[index].EnemyPool.Count == 0)
                        this._mapConfig.RoomConfigs[index].EnemyPool = new();
                    else if (this._mapConfig.RoomConfigs[index].EnemyPool.Count > 1)
                        this._mapConfig.RoomConfigs[index].EnemyPool = new() { this._mapConfig.RoomConfigs[index].EnemyPool[0] };

                    this._mapConfig.RoomConfigs[index].EnemyPool[0] = (SpawnTypeEnemy)EditorGUILayout.EnumPopup(this._mapConfig.RoomConfigs[index].EnemyPool[0]);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
            }

            foreach (var index in indicesToRemove.OrderByDescending(i => i))
                this._mapConfig.RoomConfigs.RemoveAt(index);
        }

        private void DrawTrapSpawnProperties(int roomIndex)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Traps", GUILayout.Width(80f));
            EditorGUILayout.LabelField("Min", GUILayout.Width(40f));
            this._mapConfig.RoomConfigs[roomIndex].MinTraps = EditorGUILayout.IntField(this._mapConfig.RoomConfigs[roomIndex].MinTraps, GUILayout.Width(50f));
            EditorGUILayout.LabelField("Max", GUILayout.Width(40f));
            this._mapConfig.RoomConfigs[roomIndex].MaxTraps = EditorGUILayout.IntField(this._mapConfig.RoomConfigs[roomIndex].MaxTraps, GUILayout.Width(50f));
            EditorGUILayout.EndHorizontal();

            var indicesToRemove = new List<int>();

            if (this._mapConfig.RoomConfigs[roomIndex].TrapPool.Count == 0)
                this._mapConfig.RoomConfigs[roomIndex].TrapPool.Add(SpawnTypeTrap.PLATFORM_SPIKE);

            this._trapSpawnFoldout = EditorGUILayout.Foldout(this._trapSpawnFoldout, "Types:", true);

            if (!this._trapSpawnFoldout)
                return;

            int count = this._mapConfig.RoomConfigs[roomIndex].TrapPool.Count;
            for (int index = 0; index < count; index++)
            {
                EditorGUILayout.BeginHorizontal();
                this._mapConfig.RoomConfigs[roomIndex].TrapPool[index] = (SpawnTypeTrap)EditorGUILayout.EnumPopup(this._mapConfig.RoomConfigs[roomIndex].TrapPool[index]);

                if (GUILayout.Button("-", GUILayout.Width(30)))
                    indicesToRemove.Add(this._mapConfig.RoomConfigs[roomIndex].TrapPool.Count - 1);

                if (GUILayout.Button("+", GUILayout.Width(30)))
                    this._mapConfig.RoomConfigs[roomIndex].TrapPool.Add(SpawnTypeTrap.PLATFORM_SPIKE);

                EditorGUILayout.EndHorizontal();
            }

            foreach (var index in indicesToRemove.OrderByDescending(i => i))
                this._mapConfig.RoomConfigs[roomIndex].TrapPool.RemoveAt(index);
        }

        private void DrawEnemySpawnProperties(int roomIndex)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enemies", GUILayout.Width(80f));
            EditorGUILayout.LabelField("Min", GUILayout.Width(40f));
            this._mapConfig.RoomConfigs[roomIndex].MinEnemies = EditorGUILayout.IntField(this._mapConfig.RoomConfigs[roomIndex].MinEnemies, GUILayout.Width(50f));
            EditorGUILayout.LabelField("Max", GUILayout.Width(40f));
            this._mapConfig.RoomConfigs[roomIndex].MaxEnemies = EditorGUILayout.IntField(this._mapConfig.RoomConfigs[roomIndex].MaxEnemies, GUILayout.Width(50f));
            EditorGUILayout.EndHorizontal();

            var indicesToRemove = new List<int>();

            if (this._mapConfig.RoomConfigs[roomIndex].EnemyPool.Count == 0)
                this._mapConfig.RoomConfigs[roomIndex].EnemyPool.Add(SpawnTypeEnemy.SHOOTER_ENERGY_MISSILE);

            this._enemySpawnFoldout = EditorGUILayout.Foldout(this._enemySpawnFoldout, "Types:", true);

            if (!this._enemySpawnFoldout)
                return;

            int count = this._mapConfig.RoomConfigs[roomIndex].EnemyPool.Count;
            for (int index = 0; index < count; index++)
            {
                EditorGUILayout.BeginHorizontal();
                this._mapConfig.RoomConfigs[roomIndex].EnemyPool[index] = (SpawnTypeEnemy)EditorGUILayout.EnumPopup(this._mapConfig.RoomConfigs[roomIndex].EnemyPool[index]);

                if (GUILayout.Button("-", GUILayout.Width(30)))
                    indicesToRemove.Add(this._mapConfig.RoomConfigs[roomIndex].EnemyPool.Count - 1);

                if (GUILayout.Button("+", GUILayout.Width(30)))
                    this._mapConfig.RoomConfigs[roomIndex].EnemyPool.Add(SpawnTypeEnemy.SHOOTER_ENERGY_MISSILE);

                EditorGUILayout.EndHorizontal();
            }

            foreach (var index in indicesToRemove.OrderByDescending(i => i))
                this._mapConfig.RoomConfigs[roomIndex].EnemyPool.RemoveAt(index);
        }

        private void LoadData()
        {
            this._enemyConfigs = new EnemyConfig[Enum.GetValues(typeof(SpawnTypeEnemy)).Length];
            this._weaponConfigs = new WeaponConfig[Enum.GetValues(typeof(WeaponType)).Length];
            this._enemyFoldouts = new bool[this._enemyConfigs.Length];
            this._weaponFoldouts = new bool[this._weaponConfigs.Length];

            this._playerConfig = this.LoadData<PlayerConfig>(ProjectPaths.PLAYER_CONFIG_DATABASE);
            this._enemyConfigs = this.LoadData<EnemyConfig[]>(ProjectPaths.ENEMY_CONFIG_DATABASE);
            this._weaponConfigs = this.LoadData<WeaponConfig[]>(ProjectPaths.WEAPON_CONFIG_DATABASE);
            this._mapConfig = this.LoadData<MapConfig>(ProjectPaths.MAP_CONFIG_DATABASE);
        }

        private T LoadData<T>(string path)
        {
            return Resources.Load<GameConfigDatabase<T>>(path).Config;
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
