using UnityEngine;

namespace Game
{
    public interface IRoom
    {
        bool HasVisited { get; }
        RoomType Type { get; }
        Vector3 Position { get; }
        float Size { get; }
        void ShowIfVisited();
        void HideIfPlayerIsNotHere();
    }
}
