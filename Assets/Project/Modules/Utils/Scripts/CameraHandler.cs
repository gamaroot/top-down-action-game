using UnityEngine;

namespace Utils
{
    public class CameraHandler
    {
        public Camera MainCamera { get; private set; }

        public static CameraHandler Instance { get; private set; }

        public static void Load(Camera mainCamera)
        {
            Instance = new CameraHandler
            {
                MainCamera = mainCamera
            };
        }
    }
}