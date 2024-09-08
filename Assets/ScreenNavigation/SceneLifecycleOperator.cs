using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    public class SceneLifecycleOperator : SceneContextOperator
    {
        private readonly SceneTransitionAnimator transitionAnimator = new();

        #region Observe Events
        public void ExecuteOnNextShowAnimationComplete(Action action)
        {
            SceneContext screenContext = base.GetScreenContext(x => x.SceneState == SceneState.LOADING ||
                                                                     x.SceneState == SceneState.ANIMATING_SHOW);
            this.ExecuteOnShowAnimationComplete(screenContext.ID, action);
        }

        public void ExecuteOnShowProcessStart(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnShowProcessStart = action
            });
        }

        public void ExecuteOnShowAnimationComplete(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnShowAnimationComplete = action
            });
        }

        public void ExecuteOnNextHideAnimationComplete(Action action)
        {
            SceneContext screenContext = base.GetScreenContext(x => x.SceneState == SceneState.UNLOADING ||
                                                                     x.SceneState == SceneState.ANIMATING_HIDE);

            if (!screenContext.IsEmpty)
            {
                this.ExecuteOnHideAnimationComplete(screenContext.ID, action);
            }
            else
            {
                screenContext = base.GetScreenContext(x => x.SceneState == SceneState.LOADED);
                this.ExecuteOnHideAnimationComplete(screenContext.ID, action);
            }
        }

        public void ExecuteOnHideProcessStart(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnHideProcessStart = action
            });
        }

        public void ExecuteOnHideAnimationComplete(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnHideAnimationComplete = action
            });
        }

        public void ExecuteOnNextSceneLoad(Action action)
        {
            SceneContext screenContext = base.GetScreenContext(x => x.SceneState == SceneState.LOADING ||
                                                                     x.SceneState == SceneState.ANIMATING_SHOW);
            this.ExecuteOnSceneLoad(screenContext.ID, action);
        }

        public void ExecuteOnSceneLoad(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnSceneLoad = action
            });
        }

        public void ExecuteOnNextSceneUnload(Action action)
        {
            SceneContext screenContext = base.GetScreenContext(x => x.SceneState == SceneState.UNLOADING ||
                                                                     x.SceneState == SceneState.ANIMATING_HIDE);
            this.ExecuteOnSceneUnload(screenContext.ID, action);
        }

        public void ExecuteOnSceneUnload(SceneID sceneID, Action action)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExecuteOnSceneUnload = action
            });
        }
        #endregion

        #region Private/Protected Events
        protected AsyncOperation StartShowScreenProcess(SceneLoadData data, bool clearOldSceneParams = false)
        {
            SceneContext screenContext = base.GetScreenContext(data.ID);
            if (this.IsScreenLoading(screenContext))
                return null;
            
            screenContext.ExecuteOnShowProcessStart?.Invoke();

            this.PrepareOpeningScene(data, clearOldSceneParams);

            return SceneManager.LoadSceneAsync((int)data.ID, data.Mode);
        }

        protected void OnSceneLoaded(Scene scene, LoadSceneMode _)
        {
            SceneContext screenContext = base.GetScreenContext((SceneID)scene.buildIndex);
            screenContext.ExecuteOnSceneLoad?.Invoke();

            base.UpdateScreenContext(new SceneContext
            {
                SceneName = scene.name,
                ID = (SceneID)scene.buildIndex,
                SceneState = SceneState.ANIMATING_SHOW,
                ExecuteOnSceneLoad = null
            });

            this.StartShowAnimation(scene, screenContext.ExecuteOnShowAnimationComplete);
        }

        private void StartShowAnimation(Scene scene, Action onComplete = null)
        {
            this.transitionAnimator.StartTransition(new SceneTransitionAnimationData
            {
                Scene = scene,
                IsIntro = true,
                OnComplete = () =>
                {
                    this.OnShowAnimationComplete((SceneID)scene.buildIndex);
                    onComplete?.Invoke();
                }
            });
        }

        private void OnShowAnimationComplete(SceneID sceneID)
        {
            SceneContext screenContext = base.GetScreenContext(sceneID);
            screenContext.ExecuteOnShowAnimationComplete?.Invoke();

            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                SceneState = SceneState.LOADED
            });
        }

        protected void StartHideScreenProcess(SceneID sceneID)
        {
            SceneContext screenContext = base.GetScreenContext(sceneID);
            if (this.IsScreenUnloading(screenContext))
                return;

            Scene scene = SceneManager.GetSceneByBuildIndex((int)sceneID);
            if (!scene.isLoaded)
                return;

            screenContext.ExecuteOnHideProcessStart?.Invoke();
            
            base.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                SceneState = SceneState.ANIMATING_HIDE
            });

            this.transitionAnimator.StartTransition(new SceneTransitionAnimationData
            {
                Scene = scene,
                IsIntro = false,
                OnComplete = () => this.OnHideAnimationComplete(scene)
            });
        }

        private void OnHideAnimationComplete(Scene scene)
        {
            SceneContext screenContext = base.GetScreenContext((SceneID)scene.buildIndex);
            screenContext.ExecuteOnHideAnimationComplete?.Invoke();

            this.UnloadScene(scene);
        }

        private void UnloadScene(Scene scene)
        {
            base.UpdateScreenContext(new SceneContext
            {
                ID = (SceneID)scene.buildIndex,
                SceneState = SceneState.UNLOADING
            });

            SceneManager.UnloadSceneAsync(scene);
        }

        protected void OnSceneUnloaded(Scene scene)
        {
            SceneContext screenContext = base.GetScreenContext((SceneID)scene.buildIndex);
            screenContext.ExecuteOnSceneUnload?.Invoke();

            base.UpdateScreenContext(new SceneContext
            {
                SceneName = scene.name,
                ID = (SceneID)scene.buildIndex,
                SceneState = SceneState.UNLOADED
            });
        }

        private void PrepareOpeningScene(SceneLoadData data, bool clearOldSceneParams)
        {
            if (clearOldSceneParams)
            {
                base.ClearStack();
            }

            base.UpdateScreenContext(new SceneContext
            {
                ID = data.ID,
                SceneState = SceneState.LOADING,
                ExtraInputParams = data.NewSceneParams
            });
        }

        private bool IsScreenLoading(SceneContext screenContext)
        {
            return screenContext.SceneState == SceneState.LOADING ||
                   screenContext.SceneState == SceneState.ANIMATING_SHOW;
        }

        private bool IsScreenUnloading(SceneContext screenContext)
        {
            return screenContext.SceneState == SceneState.ANIMATING_HIDE ||
                   screenContext.SceneState == SceneState.UNLOADING ||
                   screenContext.SceneState == SceneState.UNLOADED;
        }
        #endregion
    }
}
