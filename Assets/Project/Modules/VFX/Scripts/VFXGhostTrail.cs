using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class VFXGhostTrail : MonoBehaviour
    {
        [Tooltip("Time between ghost spawns")]
        [SerializeField] private float _spawnInterval = 0.05f;

        [Tooltip("Lifetime of each ghost")]
        [SerializeField] private float _lifetime = 0.5f;

        private float _timer;

        private void Update()
        {
            this._timer += Time.deltaTime;
            if (this._timer < this._spawnInterval)
                return;

            MeshRenderer ghost = SpawnablePool.SpawnPlayerGhostTrail();
            ghost.transform.position = base.transform.position;
            ghost.gameObject.SetActive(true);

            ghost.material.DOKill();
            ghost.material.DOFade(0f, this._lifetime)
                            .OnComplete(() =>
                            {
                                ghost.material.color = new Color(1f, 1f, 1f, 0.3f);
                                ghost.gameObject.SetActive(false);
                            });

            this._timer = 0f;
        }
    }
}
