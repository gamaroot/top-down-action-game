using UnityEngine;

namespace Game
{
    public class PlayerLevelUpDisplay : MonoBehaviour
    {
        public void OnShow()
        {
            base.gameObject.SetActive(true);
        }

        // Called via Animation Trigger
        public void OnHide()
        {
            base.gameObject.SetActive(false);
        }
    }
}
