﻿using System;
using Moq;
using NUnit.Framework;
using Testura.Android.Device.Services.Activity;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Tests.Device.Services
{
    [TestFixture]
    public class ActivityServiceTests
    {
        private ActivityService _activityService;
        private Mock<IAdbShellService> _adbServiceMock;

        [SetUp]
        public void SetUp()
        {
            _adbServiceMock = new Mock<IAdbShellService>();
            _activityService = new ActivityService(_adbServiceMock.Object);
        }

        [Test]
        public void Start_WhenPackageIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _activityService.Start(null, "activity", false, false));
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
            Assert.AreEqual("Unknown activity", _activityService.GetCurrent());
        }

        [Test]
        public void GetPackageVersion_WhenGetVersionFromPackageThatExist_ShouldReturnVersion()
        {
            _adbServiceMock.Setup(s => s.Shell(It.IsAny<string>())).Returns("versionName=1.1");
            var version = _activityService.GetPackageVersion("test");
            Assert.AreEqual(new Version(1, 1), version);
        }

        [Test]
        public void GetPackageVersion_WhenGetVersionFromPackageThatDontExist_ShouldReturnVersion0()
        {
            _adbServiceMock.Setup(s => s.Shell(It.IsAny<string>())).Returns(string.Empty);
            var version = _activityService.GetPackageVersion("test");
            Assert.AreEqual(new Version(0, 0), version);
        }
    }
}
