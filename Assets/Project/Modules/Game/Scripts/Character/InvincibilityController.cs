using UnityEngine;

namespace Game
{
    public class InvincibilityController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private LayerMask _defaultLayerMask;
        [SerializeField] private LayerMask _invincibilityLayerMask;
        [SerializeField] private Color _defaultPlayerMaterialColor;

        [Header("Components")]
        [SerializeField] private Material _playerMaterial;

        private void OnValidate()
        {
            this._defaultLayerMask = 1 << base.gameObject.layer;

            if (this._playerMaterial == null)
                this._playerMaterial = base.GetComponent<MeshRenderer>().material;

            if (this._defaultPlayerMaterialColor == Color.clear)
                this._defaultPlayerMaterialColor = this._playerMaterial.color;
        }

        public void ActivateInvincibility()
        {
            this._playerMaterial.color = Color.clear;
            this.UpdateLayerMask(this._invincibilityLayerMask);
        }

        public void ActivateInvincibility(float spawnInvincibilityDuration)
        {
            this.ActivateInvincibility();

            base.CancelInvoke(nameof(this.DeactivateInvincibility));
            base.Invoke(nameof(this.DeactivateInvincibility), spawnInvincibilityDuration);
        }

        public void DeactivateInvincibility()
        {
            this._playerMaterial.color = this._defaultPlayerMaterialColor;
            this.UpdateLayerMask(this._defaultLayerMask);
        }

        private void UpdateLayerMask(LayerMask layerMask)
        {
            base.gameObject.layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
        }
    }
}
