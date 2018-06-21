using Moq;
using NUnit.Framework;
using Testura.Android.Device.Server;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes;

namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class UiServiceTests
    {
        private Mock<IUiAutomatorServer> _serverMock;
        private Mock<INodeFinder> _nodeFinder;
        private Mock<INodeParser> _nodeParser;
        private UiService _uiComponent;

        [SetUp]
        public void SetUp()
        {
            _serverMock = new Mock<IUiAutomatorServer>();
            _nodeParser = new Mock<INodeParser>();
            _nodeFinder = new Mock<INodeFinder>();
            _uiComponent = new UiService(_serverMock.Object, _nodeParser.Object, _nodeFinder.Object);
        }
    }
}
