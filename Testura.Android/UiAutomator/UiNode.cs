using System.Xml.Linq;

namespace Testura.Android.UiAutomator
{
    public class UiNode
    {
        private readonly XElement node;

        public UiNode(XElement node)
        {
            this.node = node;
        }
    }
}
