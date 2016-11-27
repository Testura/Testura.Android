using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Services;
using Testura.Android.UiAutomator;
using Testura.Android.Util;

namespace Testura.Android.Tests.Integration
{
    [TestFixture]
    public class UiIntegrationTests
    {
        private AdbService adbService;
        private UiService uiService;
        private AndroidDevice device;
        private WifiService wifiService;

        [SetUp]
        public void SetUp()
        {
            adbService = new AdbService(new WindowsTerminal());
            uiService = new UiService(new NodeChecker(new ScreenDumper(new UiAutomatorServer(new WindowsTerminal()))));
            wifiService = new WifiService();
            device = new AndroidDevice(adbService, uiService, wifiService);
        }

        [Test]
        public void Integration_WhenClickingItem_ShouldClick()
        {
            var uiObject = new UiObject(device, By.Text("Kalkylator"));
            uiObject
                .WaitVisible()
                .Click();
        }
    }
}
