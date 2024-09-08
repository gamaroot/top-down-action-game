namespace ScreenNavigation
{
    internal struct CanvasAnimationData
    {
        internal float Duration;
        internal float InitialValue;
        internal float FinalValue;
        internal CanvasAnimationType Type;

        internal static CanvasAnimationData GetDataByType(CanvasAnimationType type)
        {
            return type switch
            {
                CanvasAnimationType.FADE_IN => FadeIn,
                CanvasAnimationType.FADE_OUT => FadeOut,
                _ => new CanvasAnimationData(),
            };
        }

        private static CanvasAnimationData FadeIn => new()
        {
            Type = CanvasAnimationType.FADE_IN,
            Duration = 0.3f,
            InitialValue = 0,
            FinalValue = 1f
        };

        private static CanvasAnimationData FadeOut => new()
        {
            Type = CanvasAnimationType.FADE_OUT,
            Duration = 0.3f,
            InitialValue = 1f,
            FinalValue = 0
        };
    }
}