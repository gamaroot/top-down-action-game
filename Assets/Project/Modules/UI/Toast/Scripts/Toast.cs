using TMPro;
using UnityEngine;

namespace Game
{
    public class Toast : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Message { get; private set; }

        [SerializeField] private Animator animator;

        private void OnValidate()
        {
            if (this.animator == null)
                this.animator = base.GetComponent<Animator>();
        }

        private void Start()
        {
            base.Invoke("Dismiss", 3f);
        }

        // Called through inspector on Toast click
        public void Dismiss()
        {
            this.animator.SetBool(AnimationKeys.VISIBLE, false);
        }

        // Called by Animator
        public void OnDismiss()
        {
            Destroy(base.gameObject);
        }
    }
}