using System.Collections.Generic;
using Testura.Android.UiAutomator.Search;
using Testura.Android.Util;

namespace Testura.Android.UiAutomator
{
    public class By
    {
        private By(List<NodeSearch> nodeSearches)
        {
            NodeSearches = nodeSearches;
        }

        public List<NodeSearch> NodeSearches { get; private set; }

        public static By Text(string text)
        {
            return new By(new List<NodeSearch> {new NodeSearch(new AttributeSearch(AttributeTags.Text, text))});
        }

        public static By ContainsText(string text)
        {
            return new By(new List<NodeSearch> {new NodeSearch(new AttributeSearch(AttributeTags.TextContains, text))});
        }
    }
}
