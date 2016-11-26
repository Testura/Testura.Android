using System.Threading.Tasks;

namespace Testura.Android.UiAutomator
{
    public interface IUiAutomatorServer
    {
        int LocalPort { get; }

        Task Start();

        Task<bool> Alive();

        string DumpUi();
    }
}