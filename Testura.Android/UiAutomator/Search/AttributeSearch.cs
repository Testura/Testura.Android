using Testura.Android.Util;

namespace Testura.Android.UiAutomator.Search
{
    public class AttributeSearch
    {
        public AttributeSearch(AttributeTags attributeTag, string value)
        {
            AttributeTag = attributeTag;
            Value = value;
        }

        public AttributeTags AttributeTag { get; set; }

        public string Value { get; set; }
    }
}
