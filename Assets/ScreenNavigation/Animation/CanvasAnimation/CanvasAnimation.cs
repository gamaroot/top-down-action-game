using UnityEngine;

namespace ScreenNavigation
{
    internal class CanvasAnimation : MonoBehaviour
    {
        private float progressValue;
        private CanvasAnimationData animationData;
        private IAnimationLifecycle animationLifecycle;

        public void SetupAnimation(CanvasAnimationType animationType, IAnimationLifecycle animationLifecycle)
        {
            this.animationLifecycle = animationLifecycle;
            this.animationData = CanvasAnimationData.GetDataByType(animationType);
        }

        private void Update()
        {
            if (!base.gameObject) return;

            this.progressValue += Time.unscaledDeltaTime;

            float progressValueLerp = Mathf.Lerp(this.animationData.InitialValue, this.animationData.FinalValue, this.progressValue / this.animationData.Duration);

            this.animationLifecycle.OnProgress(progressValueLerp);

            if (this.progressValue >= this.animationData.Duration)
            {
                this.animationLifecycle.OnAnimationComplete();

                Destroy(base.gameObject);
            }
        }
    }
}