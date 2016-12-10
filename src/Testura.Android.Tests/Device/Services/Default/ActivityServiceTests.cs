using System;
using Moq;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Services;
using Testura.Android.Device.Services.Default;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class ActivityServiceTests
    {
        private ActivityService _activityService;
        private Mock<IAdbService> _adbServiceMock;
        private Mock<IAndroidDevice> _androidMock;

        [SetUp]
        public void SetUp()
        {
            _adbServiceMock = new Mock<IAdbService>();
            _androidMock = new Mock<IAndroidDevice>();
            _androidMock.Setup(a => a.Adb).Returns(_adbServiceMock.Object);

            _activityService = new ActivityService();
            _activityService.InitializeServiceOwner(_androidMock.Object);
        }

        [Test]
        public void Start_WhenPackageIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _activityService.Start(null, "acivity", false, false));
        }

        [Test]
        public void Start_WhenActivityIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _activityService.Start("package", null, false, false));
        }

        [Test]
        public void Start_WhenForceStopActivity_ShouldContainForceStopActivityFlag()
        {
            _adbServiceMock.Setup(s => s.Shell(It.IsAny<string>())).Returns("test");
            _activityService.Start("package", "activity", true, false);
            _adbServiceMock.Verify(a => a.Shell("am start -W -n package/activity -S"));
        }

        [Test]
        public void Start_WhenClearTasks_ShouldContainClearTaskFlag()
        {
            _adbServiceMock.Setup(s => s.Shell(It.IsAny<string>())).Returns("test");
            _activityService.Start("package", "activity", false, true);
            _adbServiceMock.Verify(a => a.Shell("am start -W -n package/activity --activity-clear-task"), Times.Once);
        }

        [Test]
        public void Start_WhenResultsContainError_ShouldThrowException()
        {
            _adbServiceMock.Setup(a => a.Shell(It.IsAny<string>())).Returns("Starting: Intent { cmp=22/22 }\r\r\nError type 3\r\r\nError: Activity class {22/22} does not exist.\r\r\n");
            Assert.Throws<AdbException>(() => _activityService.Start("package", "activity", false, true));
        }

        [Test]
        public void GetCurrent_WhenCantParseActvity_ShouldReturnUnkownActivity()
        {
            _adbServiceMock.Setup(s => s.Shell(It.IsAny<string>())).Returns("223232ddad");
            Assert.AreEqual("Unkown activity", _activityService.GetCurrent());
        }
    }
}
