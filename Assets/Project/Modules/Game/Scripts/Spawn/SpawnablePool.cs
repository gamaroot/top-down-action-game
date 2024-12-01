using System;
using Unity.Behavior;
using UnityEngine;

namespace Game
{
    public class SpawnablePool : MonoBehaviour
    {
        [SerializeField] private GameObject _deathPrefab;
        [SerializeField] private GameObject _kamikazePrefab;
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject[] _bulletPrefabs;
        [SerializeField] private GameObject[] _bulletImpactPrefabs;
        [SerializeField] private GameObject[] _trapPrefabs;
        [SerializeField] private GameObject[] _otherPrefabs;

        private static readonly Pool[][] _pools = new Pool[7][];

        public void Init(IGameManager gameManager)
        {
            Transform baseTransform = base.transform;
            this.CreatePool(0, baseTransform, this._deathPrefab);
            this.CreatePool(1, baseTransform, this._kamikazePrefab);

            this.CreatePool(2, baseTransform, this._enemyPrefabs, (enemy) =>
            {
                enemy.GetComponent<Enemy>().Init(gameManager);
            });
            this.CreatePool(3, baseTransform, this._bulletPrefabs);
            this.CreatePool(4, baseTransform, this._bulletImpactPrefabs);
            this.CreatePool(5, baseTransform, this._trapPrefabs);
            this.CreatePool(6, baseTransform, this._otherPrefabs);
        }

        public static GameObject SpawnDeath()
        {
            return _pools[0][0].BorrowObject();
        }

        public static GameObject SpawnKamikaze()
        {
            return _pools[1][0].BorrowObject();
        }

        public static BehaviorGraphAgent SpawnEnemy(SpawnTypeEnemy type)
        {
            return _pools[2][(int)type].BorrowObject<BehaviorGraphAgent>();
        }

        public static Bullet SpawnBullet(WeaponType type)
        {
            return _pools[3][(int)type].BorrowObject<Bullet>();
        }

        public static GameObject SpawnBulletImpact(WeaponType type)
        {
            return _pools[4][(int)type].BorrowObject();
        }

        public static GameObject SpawnTrap(SpawnTypeTrap type)
        {
            return _pools[5][(int)type].BorrowObject();
        }

        public static T SpawnOther<T>(SpawnTypeOther type, float autoDisableInSeconds = -1f)
        {
            return _pools[6][(int)type].BorrowObject<T>(autoDisableInSeconds);
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

        private void CreatePool(int order, Transform baseTransform, GameObject prefabs, Action<GameObject> onObjectCreated = null)
        {
            _pools[order] = new Pool[1];
            _pools[order][0] = new(baseTransform, prefabs, onObjectCreated);
        }
    }
}