using NUnit.Framework;
using Testura.Android.Device.Server;
using Testura.Android.Util;
using Assert = NUnit.Framework.Assert;

namespace Testura.Android.Tests.Integration.Device.UiAutomator.Server
{
    [Ignore("Integration")]
    [TestFixture]
    public class UiAutomatorServerTests
    {
        private UiAutomatorServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new UiAutomatorServer(new AdbTerminal(), 9008);
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            _server.Start();
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