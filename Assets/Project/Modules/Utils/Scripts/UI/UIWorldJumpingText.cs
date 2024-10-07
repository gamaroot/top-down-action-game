using TMPro;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(TextMeshPro))]
    public class UIWorldJumpingText : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float _offsetY = 1f;
        [SerializeField] private float _jumpingSpeed = 1f;
        [SerializeField] private float _jumpingHeight = 1f;
        [SerializeField] private float _lifetime = 2f;

        [Header("Components")]
        [SerializeField, ReadOnly] private TextMeshPro _text;

        private float _timer;
        private Vector3 _direction;

        private void OnValidate()
        {
            if (this._text == null)
                this._text = base.GetComponent<TextMeshPro>();
        }

        private void Awake()
        {
            this._direction = new(0, 1f, this._offsetY);
        }

        private void OnEnable()
        {
            this._timer = 0;
            this._text.alpha = 1f;
        }

        private void Update()
        {
            if (!base.gameObject.activeSelf)
                return;

            float deltaTime = Time.deltaTime;
            this._timer += deltaTime;

            if (this._timer >= this._lifetime)
            {
                base.gameObject.SetActive(false);
                return;
            }

            this.transform.position += Mathf.Sin(this._timer * this._jumpingSpeed) * this._jumpingHeight * Time.deltaTime * this._direction;
            this._text.alpha -= deltaTime;
        }

        public void SetText(string text, Color color)
        {
            this._text.color = color;
            this._text.text = text;
        }
    }
}
