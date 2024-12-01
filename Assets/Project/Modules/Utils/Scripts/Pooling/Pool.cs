using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Pool
    {
        public int ExpandedPoolSize = 1;

        private readonly GameObject _resourcePrefab;
        private readonly Stack<GameObject> _pool = new();
        private readonly Transform _parentGameObject;
        private readonly Action<GameObject> _onObjectCreated;
        private readonly Action<GameObject> _onObjectActivated;

        public Pool(Transform parentGameObject, GameObject resourcePrefab,
                    Action<GameObject> onObjectCreated = null,
                    Action<GameObject> onObjectActivated = null)
        {
            var node = new GameObject(resourcePrefab.name);
            node.transform.SetParent(parentGameObject);

            this._parentGameObject = node.transform;
            this._resourcePrefab = resourcePrefab;
            this._onObjectCreated = onObjectCreated;
            this._onObjectActivated = onObjectActivated;

            this.AddObjectsToPool(this.ExpandedPoolSize);
        }

        public GameObject BorrowObject()
        {
            this.EnsurePoolHasObjects();

            GameObject obj = this._pool.Pop();
            this._onObjectActivated?.Invoke(obj);
            return obj;
        }

        public T BorrowObject<T>(float autoDisableInSeconds = -1f)
        {
            this.EnsurePoolHasObjects();

            GameObject obj = this._pool.Pop();
            this._onObjectActivated?.Invoke(obj);

            PoolingObject poolingObj = obj.GetComponent<PoolingObject>();
            poolingObj.SetAutoDisable(autoDisableInSeconds);

            return obj.GetComponent<T>();
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.transform.SetParent(this._parentGameObject);
            this._pool.Push(obj);
        }

        public Transform GetParent()
        {
            return this._parentGameObject;
        }

        public void DisableAll()
        {
            if (!this._parentGameObject)
                return;

            foreach (PoolingObject item in this._parentGameObject.GetComponentsInChildren<PoolingObject>())
            {
                item.Disable();
            }
        }

        private void AddObjectsToPool(int quantity)
        {
            for (int index = 0; index < quantity; index++)
            {
                var newObj = GameObject.Instantiate(this._resourcePrefab);
                newObj.name += index;
                newObj.transform.SetParent(this._parentGameObject);
                newObj.SetActive(false);

                this._onObjectCreated?.Invoke(newObj);

                PoolingObject poolingObj = newObj.GetComponent<PoolingObject>();

                if (poolingObj == null)
                    poolingObj = newObj.AddComponent<PoolingObject>();

                poolingObj.Initialize(this);

                AudioSource audioSource = newObj.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    poolingObj.SetAutoDisable(audioSource.clip.length);
                }

                ParticleSystem particleSystem = newObj.GetComponent<ParticleSystem>();
                if (particleSystem != null && !particleSystem.main.loop)
                {
                    poolingObj.SetAutoDisable(particleSystem.main.duration);
                }

                this._pool.Push(newObj);
            }
        }

        private void EnsurePoolHasObjects()
        {
            if (this._pool.Count == 0)
                this.AddObjectsToPool(this.ExpandedPoolSize);
        }
    }
}
