﻿using System;
using NUnit.Framework;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Util;

namespace Testura.Android.Tests.Device.Services
{
    [TestFixture]
    public class AdbServiceTests
    {
        private AdbService _adbService;

        [SetUp]
        public void SetUp()
        {
            _adbService = new AdbService(new AdbCommandExecutor());
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
