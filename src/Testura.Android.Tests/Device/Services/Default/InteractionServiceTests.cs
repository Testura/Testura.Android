using System;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Device.Ui.Server;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class InteractionServiceTests
    {
        
        private InteractionService _interactionService;
        private Mock<IAdbShellService> _adbServiceMock;

        [SetUp]
        public void SetUp()
        {
            _adbServiceMock = new Mock<IAdbShellService>();
            _interactionService = new InteractionService(_adbServiceMock.Object, new Mock<IInteractionUiAutomatorServer>().Object);
        }

        [Test]
        public void Tap_WhenNodeIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _interactionService.Tap(null));
        }

        [Test]
        public void InputText_WhenTextIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _interactionService.InputText(null));
        }
    }
}
