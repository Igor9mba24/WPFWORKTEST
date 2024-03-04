using NUnit.Framework;

namespace WPFWORKTEST.UnitTests
{
    [TestFixture]
    public class FunctionDataTests
    {
        [Test]
        public void XChanged_Event_IsRaised()
        {
            // Arrange
            var eventRaised = false;
            var data = new FunctionData();
            data.XChanged += (sender, args) => eventRaised = true;

            // Act
            data.X = 5.0;

            // Assert
            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void YChanged_Event_IsRaised()
        {
            // Arrange
            var eventRaised = false;
            var data = new FunctionData();
            data.YChanged += (sender, args) => eventRaised = true;

            // Act
            data.Y = 7.0;

            // Assert
            Assert.IsTrue(eventRaised);
        }

    }
}
