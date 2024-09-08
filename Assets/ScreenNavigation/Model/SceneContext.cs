using System;

namespace ScreenNavigation
{
    public struct SceneContext
    {
        public string SceneName;
        public SceneState SceneState;
        public SceneID ID;

        public object ExtraInputParams;
        public Action ExecuteOnShowProcessStart;
        public Action ExecuteOnSceneLoad;
        public Action ExecuteOnHideProcessStart;
        public Action ExecuteOnSceneUnload;
        public Action ExecuteOnShowAnimationComplete;
        public Action ExecuteOnHideAnimationComplete;

        public readonly bool IsEmpty => string.IsNullOrEmpty(this.SceneName) && 
                                        this.ID == 0;

        public SceneContext ClearAllListeners()
        {
            this.ExecuteOnShowProcessStart = null;
            this.ExecuteOnSceneLoad = null;
            this.ExecuteOnHideProcessStart = null;
            this.ExecuteOnSceneUnload = null;
            this.ExecuteOnShowAnimationComplete = null;
            this.ExecuteOnHideAnimationComplete = null;

            return this;
        }
    }
}