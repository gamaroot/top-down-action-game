using NUnit.Framework;
using ScreenNavigation;

namespace Utils.Tests
{
    public class StringFormatUtilsTests
    {
        [Test]
        public void SceneIDToLabel_ReplacesUnderscoresWithSpaces()
        {
            // Arrange
            SceneID sceneId = SceneID.INGAME_SETTINGS;

            // Act
            string result = sceneId.ToLabel();

            // Assert
            Assert.AreEqual("INGAME SETTINGS", result);
        }

        [Test]
        public void FloatToPercentage_ConvertsFloatToPercentageString()
        {
            // Arrange
            float floatValue = 0.75f;

            // Act
            string result = floatValue.ToPercentage();

            // Assert
            Assert.AreEqual("75%", result);
        }

        [Test]
        public void FloatToTimeElapsed_ConvertsFloatInSecondsToHourMinuteSecondsFormat()
        {
            // Arrange
            float timeValue = 3665.0f; // 1 hour, 1 minute, and 5 seconds

            // Act
            string result = timeValue.ToTimeElapsed();

            // Assert
            Assert.AreEqual("01:01:05", result);
        }
    }
}