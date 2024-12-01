using UnityEngine;
using Utils;

namespace Game
{
    public class MapExitPortal : MonoBehaviour
    {
        private IGameManager _gameManager;

        private void Awake()
        {
            this._gameManager = new CrossSceneReference().GetObjectByType<IGameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player.ToString()))
            {
                this._gameManager.OnPlayerEscape();
            }
        }
    }
}
