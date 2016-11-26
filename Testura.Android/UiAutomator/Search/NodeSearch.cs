using System.Collections.Generic;

namespace Testura.Android.UiAutomator.Search
{
    public class NodeSearch
    {
        public NodeSearch(IList<AttributeSearch> attributeSearches)
        {
            AttributeSearches = attributeSearches;
        }

        public NodeSearch(AttributeSearch attributeSearch)
        {
            AttributeSearches = new List<AttributeSearch> {attributeSearch};
        }

        public IList<AttributeSearch> AttributeSearches { get; set; }
    }
}
