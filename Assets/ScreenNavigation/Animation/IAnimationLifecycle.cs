namespace ScreenNavigation
{
    internal interface IAnimationLifecycle
    {
        void OnProgress(float progressValue);
        void OnAnimationComplete();
    }
}