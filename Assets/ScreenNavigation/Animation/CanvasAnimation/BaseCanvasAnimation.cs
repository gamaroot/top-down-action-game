using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
    internal class BaseCanvasAnimation
    {
        protected readonly List<CanvasGroup> canvasGroupList = new();

        private int _totalCanvasGroup;
        private readonly CanvasAnimation _canvasAnimation;

        public BaseCanvasAnimation()
        {
            string gameObjectName = "CanvasAnimation_" + DateTime.UtcNow.Ticks;
            this._canvasAnimation = new GameObject(gameObjectName).AddComponent<CanvasAnimation>();
        }

        protected void SetupAnimation(Scene scene, CanvasAnimationType animationType, IAnimationLifecycle animationLifecycle)
        {
            this._canvasAnimation.SetupAnimation(animationType, animationLifecycle);
            this.AddCanvasGroupOnCanvases(scene);
        }

        protected void SetInteractable(bool interactable)
        {
            for (int index = 0; index < this._totalCanvasGroup; index++)
            {
                canvasGroupList[index].interactable = interactable;
            }
        }

        private void AddCanvasGroupOnCanvases(Scene scene)
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            for (int index = 0; index < rootGameObjects.Length; index++)
            {
                GameObject rootGameObject = rootGameObjects[index];
                if (rootGameObject.TryGetComponent(out Canvas _))
                {
                    CanvasGroup canvasGroup = rootGameObject.GetComponent<CanvasGroup>() ?? rootGameObject.AddComponent<CanvasGroup>();
                    canvasGroup.interactable = false;
                    this.canvasGroupList.Add(canvasGroup);
                }
            }
            this._totalCanvasGroup = this.canvasGroupList.Count;
        }
    }
}