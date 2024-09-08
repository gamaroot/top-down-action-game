using System;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    internal struct SceneTransitionAnimationData
    {
        public Scene Scene;
        public bool IsIntro;
        public Action OnComplete;
    }
}