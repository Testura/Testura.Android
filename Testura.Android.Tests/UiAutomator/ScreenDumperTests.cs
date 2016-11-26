using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Testura.Android.UiAutomator;

namespace Testura.Android.Tests.UiAutomator
{
    [TestFixture]
    public class ScreenDumperTests
    {
        private ScreenDumper screenDumper;
        private Mock<IUiAutomatorServer> uiAutomatorServerMock;

        [SetUp]
        public void SetUp()
        {
            uiAutomatorServerMock = new Mock<IUiAutomatorServer>();
            screenDumper = new ScreenDumper(uiAutomatorServerMock.Object);
        }

        [Test]
        public async Task ScreenDumper_WhenDumpingUiBeforeStartingServer_ShouldStartServer()
        {
            uiAutomatorServerMock.Setup(u => u.Alive()).ReturnsAsync(false);
            uiAutomatorServerMock.Setup(u => u.DumpUi()).Returns("<test> hej </test>");
            await screenDumper.DumpUi();
            uiAutomatorServerMock.Verify(u => u.Start(), Times.Once);
        }

        [Test]
        public async Task ScreenDumper_WhenDumpingUi_ShouldReturnXDocument()
        {
            var xml = "<test> hej </test>";
            uiAutomatorServerMock.Setup(u => u.Alive()).ReturnsAsync(true);
            uiAutomatorServerMock.Setup(u => u.DumpUi()).Returns(xml);
            var result = await screenDumper.DumpUi();
            Assert.AreEqual(xml, result);           
        }
    }
}
