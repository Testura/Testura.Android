using System;
using System.Collections.Generic;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Server;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Util;
using Testura.Android.Util.Walker;
using Testura.Android.Util.Walker.Cases.Stop;
using Testura.Android.Util.Walker.Cases.Tap;
using Testura.Android.Util.Walker.Cases.Time;
using Testura.Android.Util.Walker.Input;
using Assert = NUnit.Framework.Assert;

namespace Testura.Android.Tests.Integration.Device.UiAutomator.Server
{
    [TestFixture]
    public class UiAutomatorServerTests
    {
        private UiAutomatorServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new UiAutomatorServer(new AdbCommandExecutor(), 9008);
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            var d = new AndroidDevice();
            var o = new List<TapCase>
            {
                new FuncTapCase(new List<Where> {Where.ContentDesc("MainSettingsView_Logout") }, (device, node) => false),
                new FuncTapCase(new List<Where> {Where.ResourceId("com.android.systemui:id/back") }, (device, node) => false),
                new FuncTapCase(new List<Where> {Where.ResourceId("com.android.systemui:id/home") }, (device, node) => false),
                new FuncTapCase(new List<Where> {Where.ResourceId("com.android.systemui:id/recent_apps") }, (device, node) => false)
            };

            var inputs = new List<IAppWalkerInput>
            {
                new TapAppWalkerInput(
                    true,
                    o)
            };

            var appWalker = new AppWalker(new AppWalkerConfiguration { ShouldStartActivity = false }, inputs);

            appWalker.Start(
                d,
                null,
                null,
                new List<TimeCase>(),
                new List<StopCase>());
        }

        [Test]
        public void UiAUtomatorServer_WhenDumpingUi_ShouldGetContentBack()
        {
            _server.Start();
            var ui = _server.DumpUi();
            Assert.IsNotNull(ui);
            Assert.IsNotEmpty(ui);
        }
    }
}