using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    internal class CanvasFadeAnimation : BaseCanvasAnimation, IAnimationLifecycle
    {
        private Action onComplete;

        #region IAnimationLifecycle
        public void OnProgress(float progressValue)
        {
            foreach (CanvasGroup item in base.canvasGroupList)
            {
                if (item != null)
                    item.alpha = progressValue;
            }
        }

        public void OnAnimationComplete()
        {
            base.SetInteractable(true);

            Action onCompleteRef = this.onComplete;
            this.onComplete = null;

            onCompleteRef?.Invoke();
        }
        #endregion

        public void StartAnimationOnScene(Scene scene, CanvasAnimationType animationType, Action onComplete)
        {
            this.onComplete = onComplete;
            base.SetupAnimation(scene, animationType, this);
        }
    }
}