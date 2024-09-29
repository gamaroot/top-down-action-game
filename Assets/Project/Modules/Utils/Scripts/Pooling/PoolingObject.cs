using UnityEngine;

namespace Game
{
    public class PoolingObject : MonoBehaviour
    {
        private Pool _pool;
        private float _autoDisableTimer = -1f;

        private void OnEnable()
        {
            base.transform.SetParent(this._pool.GetParent());

            if (this._autoDisableTimer > 0)
                base.Invoke(nameof(Disable), this._autoDisableTimer);
        }

        private void OnDisable()
        {
            this._pool?.ReturnToPool(base.gameObject);
        }

        public void Initialize(Pool resourcePool)
        {
            this._pool = resourcePool;
        }

        public void Disable()
        {
            base.gameObject.SetActive(false);
        }

        public void SetAutoDisable(float time)
        {
            this._autoDisableTimer = time;
        }
    }
}
