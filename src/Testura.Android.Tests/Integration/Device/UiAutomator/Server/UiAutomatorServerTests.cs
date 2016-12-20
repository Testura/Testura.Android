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
    //[Ignore("Integration")]
    public class UiAutomatorServerTests
    {
        private UiAutomatorServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new UiAutomatorServer(new WindowsTerminal(new DeviceConfiguration()), 9008);
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            _server.Start();
        }

        [Test]
        public void UiAUtomatorServer_WhenDumpingUi_ShouldGetContentBack()
        {
            var device = new AndroidDevice(new DeviceConfiguration { ShouldInstallApk = false, Serial = "00a8f2cb968d4543" });
            device.Adb.Push(@"C:\Users\Mille\Desktop\SuperTest\woo oh\mm1.txt", "/sdcard/");

            //_server.Start();
            //var ui = _server.DumpUi();
            //Assert.IsNotNull(ui);
            //Assert.IsNotEmpty(ui);
        }
    }
}
