using UnityEngine;

namespace Game
{
    public class PlayerLevelUpDisplay : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void OnShow()
        {
            this._animator.SetBool(AnimationKeys.VISIBLE, true);
        }

        // Called via Animation Trigger
        public void OnHide()
        {
            this._animator.SetBool(AnimationKeys.VISIBLE, false);
        }
    }
}
