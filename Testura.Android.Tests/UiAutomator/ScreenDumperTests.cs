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
        public void ScreenDumper_WhenDumpingUiBeforeStartingServer_ShouldStartServer()
        {
            uiAutomatorServerMock.Setup(u => u.Alive(2)).Returns(false);
            uiAutomatorServerMock.Setup(u => u.DumpUi()).Returns("<test> hej </test>");
            screenDumper.DumpUi();
            uiAutomatorServerMock.Verify(u => u.Start(), Times.Once);
        }

        [Test]
        public void ScreenDumper_WhenDumpingUi_ShouldReturnXDocument()
        {
            var xml = "<test> hej </test>";
            uiAutomatorServerMock.Setup(u => u.Alive(2)).Returns(true);
            uiAutomatorServerMock.Setup(u => u.DumpUi()).Returns(xml);
            var result = screenDumper.DumpUi();
            Assert.AreEqual(xml, result);           
        }
    }
}
