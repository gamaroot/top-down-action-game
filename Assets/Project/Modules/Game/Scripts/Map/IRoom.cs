namespace Game
{
    public interface IRoom
    {
        bool HasVisited { get; }
        void ShowIfVisited();
        void HideIfPlayerIsNotHere();
    }
}
