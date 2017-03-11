using System;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class InteractionServiceTests
    {
        
        private InteractionService _interactionService;
        private Mock<IAdbService> _adbServiceMock;
        private Mock<IAndroidDevice> _androidMock;

        [SetUp]
        public void SetUp()
        {
            _androidMock = new Mock<IAndroidDevice>();
            _adbServiceMock = new Mock<IAdbService>();
            _androidMock.Setup(a => a.Adb).Returns(_adbServiceMock.Object);

            _interactionService = new InteractionService();
        }

        [Test]
        public void Click_WhenNodeIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _interactionService.Tap(null));
        }

        [Test]
        public void SendKeys_WhenTextIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _interactionService.InputText(null));
        }
    }
}
