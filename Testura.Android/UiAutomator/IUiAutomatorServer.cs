using System.Threading.Tasks;

namespace Testura.Android.UiAutomator
{
    public interface IUiAutomatorServer
    {
        int LocalPort { get; }

        void Start();

        bool Alive(int timeout);

        string DumpUi();
    }
}