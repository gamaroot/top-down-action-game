using NUnit.Framework;

namespace Utils.Tests
{
    public class EnumUtilsTests
    {
        public enum ExampleEnum
        {
            Option1,
            Option2,
            Option3
        }

        [Test]
        public void Length_ReturnsCorrectEnumLength()
        {
            // Arrange
            int expectedLength = 3; // Number of enum values in ExampleEnum

            // Act
            int resultLength = EnumUtils<ExampleEnum>.Length();

            // Assert
            Assert.AreEqual(expectedLength, resultLength);
        }
    }
}