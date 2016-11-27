using System.Collections.Generic;
using System.Xml.Linq;

namespace Testura.Android.UiAutomator
{
    public interface INodeChecker
    {
        XElement GetNodeBy(By by);
        IList<XElement> GetNodesBy(By by);
    }
}