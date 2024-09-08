using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class CrossSceneReference
    {
        private readonly int _sceneBuildIndex;

        public CrossSceneReference() { }

        public CrossSceneReference(int sceneBuildIndex)
        {
            this._sceneBuildIndex = sceneBuildIndex;
        }

        public T GetObjectByType<T>()
        {
            T referencedObject = default;

            if (this._sceneBuildIndex < SceneManager.sceneCountInBuildSettings)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(this._sceneBuildIndex);

                if (scene.isLoaded)
                {
                    GameObject[] rootGameObjects = scene.GetRootGameObjects();
                    for (int index = 0; index < rootGameObjects.Length; index++)
                    {
                        referencedObject = rootGameObjects[index].GetComponent<T>();

                        if (referencedObject != null) break;
                    }
                }
            }

            return referencedObject;
        }
    }
}