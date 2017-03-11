using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Terminal;
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
            _server = new UiAutomatorServer(new Terminal(new DeviceConfiguration()), 9008);
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            _server.Start();
        }

        [Test]
        public void UiAUtomatorServer_WhenDumpingUi_ShouldGetContentBack()
        {
            //_server.Start();
            //var ui = _server.DumpUi();
            //Assert.IsNotNull(ui);
            //Assert.IsNotEmpty(ui);

            var device = new AndroidDevice(new DeviceConfiguration());
            device.Ui.CreateUiObject(With.ResourceId("android:id/search_src_text")).SendKeys("171");
        }
    }
}