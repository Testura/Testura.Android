using NUnit.Framework;
using System.Threading.Tasks;
using Testura.Android.UiAutomator;
using Testura.Android.Util;

namespace Testura.Android.Tests.UiAutomator
{
    [TestFixture]
    class UiAutomatorServerTests
    {
        private UiAutomatorServer server;

        [SetUp]
        public void SetUp()
        {
            server = new UiAutomatorServer(new WindowsTerminal());
        }

        [Test]
        public void UiAutomatorServer_WhenStartingServer_ShouldNotThrowException()
        {
            server.Start();
        }

        [Test]
        public void UiAUtomatorServer_WhenDumpingUi_ShouldGetContentBack()
        {
            server.Start();
            var ui = server.DumpUi();
            Assert.IsNotNull(ui);
            Assert.IsNotEmpty(ui);
        }
    }
}
