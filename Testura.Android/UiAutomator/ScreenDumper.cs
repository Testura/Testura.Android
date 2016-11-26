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

        public async Task<XDocument> DumpUi()
        {
            var alive = await server.Alive();
            if (!alive)
            {
                await server.Start();
            }
            return XDocument.Parse(server.DumpUi());
        }
    }
}