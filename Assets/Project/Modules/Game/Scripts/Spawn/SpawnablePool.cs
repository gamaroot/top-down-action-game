using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnablePool : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;

        private static Pool[] _pools;

        private void Awake()
        {
            Transform baseTransform = base.transform;
            _pools = new Pool[this._prefabs.Length];
            for (int index = 0; index < this._prefabs.Length; index++)
            {
                _pools[index] = new(baseTransform, this._prefabs[index]);
            }
        }

        public static T Spawn<T>(SpawnType type)
        {
            return _pools[(int)type].BorrowObject<T>();
        }

        public static void DisableAll()
        {
            for (int index = 0; index < _pools.Length; index++)
            {
                _pools[index].DisableAll();
            }
        }
    }
}