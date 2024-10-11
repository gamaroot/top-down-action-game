using UnityEngine;

namespace Game
{
    public class ToastHandler : MonoBehaviour
    {
        [SerializeField] private Toast _prefab;

        private Toast _toast;

        public void Show(string message)
        {
            if (this._toast)
                Destroy(this._toast.gameObject);

            this._toast = Instantiate(this._prefab);
            this._toast.Message.text = message;
            this._toast.transform.SetParent(base.transform, false);
        }
    }
}