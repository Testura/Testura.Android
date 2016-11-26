using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testura.Android.UiAutomator
{
    public interface IScreenDumper
    {
        Task<XDocument> DumpUi();
    }
}