using UnityEngine;
using Utils;

namespace Game
{
    public class EnemyKamikaze : Enemy
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (base._meshRenderer.enabled && // Only if the enemy is still alive
                collision.gameObject.CompareTag(Tags.Player.ToString()))
            {
                this.OnPrepareForSelfDestruction();
            }
        }

        private void OnEnable()
        {
            this._movement.enabled = true;
            this._meshRenderer.enabled = true;
        }

        private void OnPrepareForSelfDestruction()
        {
            this._movement.enabled = false;
            this._meshRenderer.enabled = false;

            SFX.PlayExplosion(SFXTypeExplosion.KAMIKAZE_EXPLOSION);

            ParticleSystem explosion = SpawnablePool.SpawnExplosion<ParticleSystem>(SpawnTypeExplosion.KAMIKAZE_EXPLOSION);
            explosion.transform.position = base.transform.position;
            explosion.gameObject.SetActive(true);

            base.Invoke(nameof(this.OnSelfDestroyed), explosion.main.duration);
        }

        private void OnSelfDestroyed()
        {
            base.gameObject.SetActive(false);
        }
    }
}
