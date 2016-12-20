using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Services.Default;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class AdbServiceTests
    {
        private AdbService _adbService;
        private Mock<ITerminal> _terminalMock;
        private Mock<IAndroidDevice> _androidMock;
        private DeviceConfiguration _deviceConfiguration;

        [SetUp]
        public void SetUp()
        {
            _deviceConfiguration = new DeviceConfiguration();
            _terminalMock = new Mock<ITerminal>();
            _androidMock = new Mock<IAndroidDevice>();
            _androidMock.Setup(a => a.Configuration).Returns(_deviceConfiguration);

            _adbService = new AdbService(_terminalMock.Object);
            _adbService.InitializeServiceOwner(_androidMock.Object);
        }

        [Test]
        public void Shell_WhenCommandIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.Shell(null));
        }

        [Test]
        public void Push_WhenLocalPathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.Push(null, "path/mm"));
        }

        [Test]
        public void Push_WhenRemotePathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.Push("path/mm", null));
        }

        [Test]
        public void Pull_WhenLocalPathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.Push("path/mm", null));
        }

        [Test]
        public void Pull_WhenRemotePathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.Push(null, "path/mm"));
        }

        [Test]
        public void InstallApp_WhenPathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _adbService.InstallApp(null));
        }

    }
}
