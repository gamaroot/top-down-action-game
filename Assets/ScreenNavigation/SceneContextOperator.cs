using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    public class SceneContextOperator
    {
        private readonly List<SceneContext> _screenContext = new();

        private Action<SceneID, SceneState> screenStateChangeListener;

        public void AddListenerOnScreenStateChange(Action<SceneID, SceneState> onChange)
        {
            this.screenStateChangeListener += onChange;
        }

        public void RemoveListenerOnScreenStateChange(Action<SceneID, SceneState> onChange)
        {
            this.screenStateChangeListener -= onChange;
        }

        public void RemoveAllScreensListeners()
        {
            for (int index = 0; index < this._screenContext.Count; index++)
            {
                SceneContext updatedScreenContext = this._screenContext[index].ClearAllListeners();
                this._screenContext[index] = updatedScreenContext;
            }
        }

        #region Utilities
        public SceneID TopSceneSceneBuildIndex => this.GetTopScreenContext().ID;

        public bool IsThereOpenedScreen => !this.GetScreenContext(x => x.SceneState == SceneState.LOADED || 
                                                                       x.SceneState == SceneState.LOADING).IsEmpty;
        public bool IsSceneOpened(SceneID sceneID)
        {
            SceneState state = this.GetScreenContext(x => x.ID == sceneID).SceneState;
            return state == SceneState.LOADED || state == SceneState.LOADING;
        }

        public object GetSceneParams()
        {
            return this.GetSceneParams(this.TopSceneSceneBuildIndex);
        }

        public object GetSceneParams(SceneID sceneID)
        {
            SceneContext context = this.GetScreenContext(sceneID);
            return context.ExtraInputParams;
        }

        public void SetSceneParams(SceneID sceneID, object newSceneParams)
        {
            this.UpdateScreenContext(new SceneContext
            {
                ID = sceneID,
                ExtraInputParams = newSceneParams
            });
        }
        #endregion

        #region Private/Protected Methods
        protected void UpdateScreenContext(SceneContext updatedContext)
        {
            SceneContext updatedContextRef = updatedContext;

            SceneContext context = updatedContextRef;
            if (this._screenContext.Exists(x => x.ID == updatedContextRef.ID))
            {
                context = this._screenContext.Find(x => x.ID == updatedContextRef.ID);

                // It's empty when unloading the scene
                if (!string.IsNullOrEmpty(updatedContextRef.SceneName))
                    context.SceneName = updatedContextRef.SceneName;

                if (context.SceneState != updatedContext.SceneState)
                    screenStateChangeListener?.Invoke(context.ID, updatedContext.SceneState);

                if (updatedContextRef.SceneState != SceneState.NONE)
                    context.SceneState = updatedContextRef.SceneState;

                if (updatedContextRef.SceneState == SceneState.UNLOADED)
                {
                    context.ExtraInputParams = null;
                    context.ExecuteOnSceneLoad = null;
                    context.ExecuteOnSceneUnload = null;
                    context.ExecuteOnShowProcessStart = null;
                    context.ExecuteOnHideProcessStart = null;
                    context.ExecuteOnShowAnimationComplete = null;
                    context.ExecuteOnHideAnimationComplete = null;
                }
                else
                {
                    context.ExtraInputParams = updatedContextRef.ExtraInputParams ?? context.ExtraInputParams;

                    if (updatedContextRef.ExecuteOnSceneLoad != null)
                        context.ExecuteOnSceneLoad += updatedContextRef.ExecuteOnSceneLoad;

                    if (updatedContextRef.ExecuteOnSceneUnload != null)
                        context.ExecuteOnSceneUnload += updatedContextRef.ExecuteOnSceneUnload;

                    if (updatedContextRef.ExecuteOnShowProcessStart != null)
                        context.ExecuteOnShowProcessStart += updatedContextRef.ExecuteOnShowProcessStart;

                    if (updatedContextRef.ExecuteOnHideProcessStart != null)
                        context.ExecuteOnHideProcessStart += updatedContextRef.ExecuteOnHideProcessStart;

                    if (updatedContextRef.ExecuteOnShowAnimationComplete != null)
                        context.ExecuteOnShowAnimationComplete += updatedContextRef.ExecuteOnShowAnimationComplete;

                    if (updatedContextRef.ExecuteOnHideAnimationComplete != null)
                        context.ExecuteOnHideAnimationComplete += updatedContextRef.ExecuteOnHideAnimationComplete;
                }

                int index = this._screenContext.FindIndex(x => x.ID == updatedContextRef.ID);
                this._screenContext[index] = context;
            }
            else
            {
                context = new SceneContext()
                {
                    ID = updatedContext.ID,
                    SceneState = SceneState.NONE

                };
                this._screenContext.Add(updatedContextRef);

                screenStateChangeListener?.Invoke(updatedContext.ID, updatedContext.SceneState);
            }
#if UNITY_EDITOR
            if (updatedContext.SceneState != SceneState.NONE &&
                updatedContext.SceneState != context.SceneState)
            {
                Debug.Log($"[ScreenNavigation] Updating scene state: {context.SceneState} => {updatedContext.SceneState}");
            }
#endif
        }

        protected SceneContext GetScreenContext(SceneID sceneID)
        {
            return this._screenContext.Find(x => x.ID == sceneID);
        }

        protected SceneContext GetScreenContext(Predicate<SceneContext> filter)
        {
            return this._screenContext.Find(filter);
        }

        protected SceneContext GetTopScreenContext()
        {
            Scene topMostScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            return this.GetScreenContext((SceneID)topMostScene.buildIndex);
        }

        protected void ClearStack()
        {
            this._screenContext.Clear();
            Debug.Log($"[ScreenNavigation] Clearing all saved scene params");
        }
        #endregion
    }
}