using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.TestTools;
using Utils;

namespace Game.Tests
{
    public class PlayerWeaponTests
    {
        private GameObject _eventSystem;
        private CameraHandler _cameraHandler;

        [SetUp]
        public void Setup()
        {
            this.CreateEventSystem();
        }

        [UnityTest]
        public IEnumerator Shoot_WithBasicBullets_FiresShootEvent()
        {
            yield return new WaitForEndOfFrame();

            // Arrange

            // Act

            // Assert
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(this._eventSystem);
            GameObject.Destroy(this._cameraHandler.gameObject);

            this._eventSystem = null;
            this._cameraHandler = null;
        }

        private void CreateEventSystem()
        {
            this._eventSystem = new GameObject();
            this._eventSystem.AddComponent<EventSystem>();
            this._eventSystem.AddComponent<InputSystemUIInputModule>();
        }
    }
}
