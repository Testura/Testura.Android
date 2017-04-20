using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;
using Testura.Android.Device;
using Testura.Android.Device.Configurations;
using Testura.Android.Device.Ui.Search;
using Testura.Android.Device.Ui.Server;
using Testura.Android.Util.Terminal;
using Testura.Android.Util.Walker;
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

            var monkey = new AppWalker(new AppWalkerConfiguration { ShouldOnlyTapClickAbleNodes = true});
            monkey.Start(new AndroidDevice(), "se.ST.Singapore.Inspection.Feature", "md5a2f3c6b761ca1b6d9d39bd996b5b364c.StartActivity", new List<WalkerTrigger> { new WalkerTrigger
            {
                Withs = new List<With> { With.ContentDesc("MainSettingsView_Logout")},
                Case = (device, node) => false
            }});
        }
    }
}