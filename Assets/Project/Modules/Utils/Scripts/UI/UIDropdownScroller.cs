using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class UIDropdownScroller : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private float _scrollPosition = 1f;

        private void Start()
        {
            this._scrollRect = this.GetComponentInParent<ScrollRect>(true);

            int childCount = this._scrollRect.content.transform.childCount - 1;
            int childIndex = base.transform.GetSiblingIndex();

            childIndex = childIndex < ((float)childCount / 2f) ? childIndex - 1 : childIndex;

            this._scrollPosition = 1f - ((float)childIndex / childCount);
        }

        public void OnSelect(int _)
        {
            if (this._scrollRect)
                this._scrollRect.verticalScrollbar.value = this._scrollPosition;
        }
    }
}
