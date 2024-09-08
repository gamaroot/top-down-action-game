namespace ScreenNavigation
{
    internal class SceneTransitionAnimator
    {
        public void StartTransition(SceneTransitionAnimationData transitionData)
        {
                    new CanvasFadeAnimation()
                            .StartAnimationOnScene(transitionData.Scene,
                                                   transitionData.IsIntro ? CanvasAnimationType.FADE_IN : CanvasAnimationType.FADE_OUT,
                                                   transitionData.OnComplete);
        }
    }
}