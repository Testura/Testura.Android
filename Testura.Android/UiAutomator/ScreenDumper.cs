using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testura.Android.UiAutomator
{
    public class ScreenDumper : IScreenDumper
    {
        private readonly IUiAutomatorServer server;

        public ScreenDumper(IUiAutomatorServer server)
        {
            this.server = server;
        }

        public XDocument DumpUi()
        {
            var alive = server.Alive(2);
            if (!alive)
            {
                server.Start();
            }

            return XDocument.Parse(server.DumpUi());
        }
    }
}