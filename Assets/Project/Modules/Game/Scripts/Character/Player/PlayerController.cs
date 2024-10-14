using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerHealthController _healthController;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerParryController _parryController;
        [SerializeField] private PlayerShootController _shootController;

        private void OnValidate()
        {
            if (this._healthController == null)
                this._healthController = this.GetComponent<PlayerHealthController>();

            if (this._movementController == null)
                this._movementController = this.GetComponent<PlayerMovementController>();

            if (this._parryController == null)
                this._parryController = this.GetComponent<PlayerParryController>();

            if (this._shootController == null)
                this._shootController = this.GetComponent<PlayerShootController>();
        }

        public void Init(IGameManager gameManager)
        {
            this._healthController.Init(gameManager.PlayerConfig);
            this._movementController.Init(gameManager.PlayerConfig);
            this._parryController.Init(gameManager.PlayerConfig);
            this._shootController.Init(gameManager.WeaponConfig);
        }
    }
}
