using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CanvasRoomDisplay : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [SerializeField] private Image _image;
        
        private readonly Color[] _colors = new[] { Color.blue, Color.green, Color.red, Color.grey };

        public Transform Setup(IRoom room)
        {
            this.RectTransform.anchoredPosition = this.RectTransform.sizeDelta * new Vector2(room.Position.x, room.Position.z) / room.Size;

            Color color = room.HasVisited ? this._colors[(int)room.Type] : Color.black;
            this._image.color = color;

            return base.transform;
        }
    }
}
