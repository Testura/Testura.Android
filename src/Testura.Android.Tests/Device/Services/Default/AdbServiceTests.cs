//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using NUnit.Framework;
//using Testura.Android.Device;
//using Testura.Android.Device.Configurations;
//using Testura.Android.Device.Services.Default;
//using Testura.Android.Util.Exceptions;
//using Testura.Android.Util.Terminal;

//namespace Testura.Android.Tests.Device.Services.Default
//{
//    [TestFixture]
//    public class AdbServiceTests
//    {
//        private AdbService _adbService;
//        private Mock<ITerminal> _terminalMock;
//        private Mock<IAndroidDevice> _androidMock;
//        private DeviceConfiguration _deviceConfiguration;

//        [SetUp]
//        public void SetUp()
//        {
//            _deviceConfiguration = new DeviceConfiguration();
//            _terminalMock = new Mock<ITerminal>();
//            _androidMock = new Mock<IAndroidDevice>();
//            //_androidMock.Setup(a => a.)..Returns(_deviceConfiguration);

//            _adbService = new AdbService(_terminalMock.Object);
//            _adbService.InitializeServiceOwner(_androidMock.Object);
//        }

//        [Test]
//        public void Shell_WhenCommandIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.Shell(null));
//        }

//        [Test]
//        public void Push_WhenLocalPathIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.Push(null, "path/mm"));
//        }

//        [Test]
//        public void Push_WhenRemotePathIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.Push("path/mm", null));
//        }

//        [Test]
//        public void Push_WhenPushFileAndGettingError_ShouldThrowException()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand("adb push \"fake\" \"fake\"")).Returns("adb: error: remote object 'fake' does not exist");
//            Assert.Throws<AdbException>(() => _adbService.Push("fake", "fake"));
//        }

//        [Test]
//        public void Pull_WhenLocalPathIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.Push("path/mm", null));
//        }

//        [Test]
//        public void Pull_WhenRemotePathIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.Push(null, "path/mm"));
//        }

//        [Test]
//        public void Pull_WhenPullingFileAndGettingError_ShouldThrowException()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand("adb pull \"fake\" \"fake\"")).Returns("adb: error: remote object 'fake' does not exist");
//            Assert.Throws<AdbException>(() => _adbService.Pull("fake", "fake"));
//        }

//        [Test]
//        public void InstallApp_WhenPathIsNull_ShouldThrowException()
//        {
//            Assert.Throws<ArgumentException>(() => _adbService.InstallApp(null));
//        }

//        [Test]
//        public void InstallApp_WhenInstallingAppWithReinstallFlag_ShouldContainFlag()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand(It.IsAny<string>())).Returns("return");
//            _adbService.InstallApp("my/apk");
//            _terminalMock.Verify(t => t.ExecuteCommand("adb install -r \"my/apk\""), Times.Once());
//        }

//        [Test]
//        public void InstallApp_WhenInstallingAppWithoutReinstallFlag_ShouldNotContainFlag()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand(It.IsAny<string>())).Returns("return");
//            _adbService.InstallApp("my/apk", false);
//            _terminalMock.Verify(t => t.ExecuteCommand("adb install \"my/apk\""), Times.Once());
//        }

//        [Test]
//        public void Shell_WhenConfigurationContainsSerial_AbdCommandShouldUseSerialFlag()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand(It.IsAny<string>())).Returns("return");
//            _deviceConfiguration.Serial = "2324";
//            _adbService.Shell("command");
//            _terminalMock.Verify(t => t.ExecuteCommand("adb -s 2324 shell \"command\""));
//        }

//        [Test]
//        public void Shell_WhenConfigurationContainsAdbPath_AdbCommandShouldContainFullPathToAdb()
//        {
//            _terminalMock.Setup(t => t.ExecuteCommand(It.IsAny<string>())).Returns("return");
//            _deviceConfiguration.AdbPath = "my/adb.exe";
//            _adbService.Shell("command");
//            _terminalMock.Verify(t => t.ExecuteCommand("my/adb.exe shell \"command\""));
//        }
//    }
//}
