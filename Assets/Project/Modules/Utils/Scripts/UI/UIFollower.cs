using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class UIFollower : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _offset = new(0, 2);

        private void OnValidate()
        {
            if (this._rectTransform == null)
                this._rectTransform = this.GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            Vector3 screenPosition = CameraHandler.Instance.MainCamera.WorldToScreenPoint(this._target.position);
            base.transform.position = screenPosition;

            this._rectTransform.anchoredPosition += this._offset;
        }

        public void SetTarget(Transform target)
        {
            this._target = target;
        }
    }
}