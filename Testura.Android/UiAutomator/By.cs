using System.Collections.Generic;
using Testura.Android.UiAutomator.Search;
using Testura.Android.Util;

namespace Testura.Android.UiAutomator
{
    public static class By
    {
        public static IList<NodeSearch> Text(string text)
        {
            return new List<NodeSearch> { new NodeSearch(new AttributeSearch(AttributeTags.Text, text)) };
        }

        public static IList<NodeSearch> ContainsText(string text)
        {
            return new List<NodeSearch> { new NodeSearch(new AttributeSearch(AttributeTags.TextContains, text)) };
        }
    }
}
