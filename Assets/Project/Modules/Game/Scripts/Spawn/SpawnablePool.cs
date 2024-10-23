using System;
using UnityEngine;

namespace Game
{
    public class SpawnablePool : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject[] _projectilePrefabs;
        [SerializeField] private GameObject[] _explosionPrefabs;
        [SerializeField] private GameObject[] _trapPrefabs;
        [SerializeField] private GameObject[] _otherPrefabs;

        private static readonly Pool[][] _pools = new Pool[5][];

        public void Init(IGameManager gameManager)
        {
            Transform baseTransform = base.transform;
            this.CreatePool(0, baseTransform, this._enemyPrefabs, (enemy) =>
            {
                enemy.GetComponent<Enemy>().Init(gameManager);
            });
            this.CreatePool(1, baseTransform, this._projectilePrefabs);
            this.CreatePool(2, baseTransform, this._explosionPrefabs);
            this.CreatePool(3, baseTransform, this._trapPrefabs);
            this.CreatePool(4, baseTransform, this._otherPrefabs);
        }

        public static T SpawnEnemy<T>(SpawnTypeEnemy type)
        {
            return _pools[0][(int)type].BorrowObject<T>();
        }

        public static T SpawnProjectile<T>(SpawnTypeProjectile type, float autoDisableInSeconds = -1f)
        {
            return _pools[1][(int)type].BorrowObject<T>(autoDisableInSeconds);
        }

        public static T SpawnExplosion<T>(SpawnTypeExplosion type, float autoDisableInSeconds = -1f)
        {
            return _pools[2][(int)type].BorrowObject<T>(autoDisableInSeconds);
        }

        public static T SpawnTrap<T>(SpawnTypeTrap type, float autoDisableInSeconds = -1f)
        {
            return _pools[3][(int)type].BorrowObject<T>(autoDisableInSeconds);
        }

        public static T SpawnOther<T>(SpawnTypeOther type, float autoDisableInSeconds = -1f)
        {
            return _pools[4][(int)type].BorrowObject<T>(autoDisableInSeconds);
        }

        public static void DisableAll()
        {
            for (int poolIndex = 0; poolIndex < _pools.Length; poolIndex++)
            {
                for (int elementIndex = 0; elementIndex < _pools[poolIndex].Length; elementIndex++)
                {
                    _pools[poolIndex][elementIndex].DisableAll();
                }
            }
        }

        private void CreatePool(int order, Transform baseTransform, GameObject[] prefabs, Action<GameObject> onObjectCreated = null)
        {
            _pools[order] = new Pool[prefabs.Length];
            for (int index = 0; index < prefabs.Length; index++)
            {
                _pools[order][index] = new(baseTransform, prefabs[index], onObjectCreated);
            }
        }
    }
}