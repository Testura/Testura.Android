using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Testura.Android.Util;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.UiAutomator
{
    public class NodeChecker : INodeChecker
    {
        private const string NotEqualTo = "!";
        private const string WildCard = "**";
        private readonly IScreenDumper screenDumper;

        public NodeChecker(IScreenDumper screenDumper)
        {
            this.screenDumper = screenDumper;
        }

        public XElement GetNodeBy(By by)
        {
            var nodes = GetNodesBy(by);
            if (nodes.Any())
            {
                return nodes.First();
            }

            throw new UiNodeNotFoundException(by);
        }

        public IList<XElement> GetNodesBy(By by)
        {
            var dump = screenDumper.DumpUi();
            var approvedNodes = new List<XElement>();
            foreach (var nodeSearch in by.NodeSearches)
            {
                foreach (var node in dump.Descendants("node"))
                {
                    var nodeOk = true;
                    foreach (var attributeSearch in nodeSearch.AttributeSearches)
                    {
                        var wildCard = false;
                        var equalTo = true;

                        // ! at the start of a value means "not equal".
                        if (attributeSearch.Value.StartsWith(NotEqualTo))
                        {
                            equalTo = false;
                            attributeSearch.Value = attributeSearch.Value.Substring(1, attributeSearch.Value.Length - 1);
                        }

                        // ** at the end of a value means wildcard
                        if (attributeSearch.Value.EndsWith(WildCard))
                        {
                            wildCard = true;
                            attributeSearch.Value.Remove(attributeSearch.Value.Length - 2, 2);
                        }

                        if (!CheckNode(node, attributeSearch.AttributeTag, attributeSearch.Value, equalTo, wildCard))
                        {
                            nodeOk = false;
                            break;
                        }

                        approvedNodes.Add(node);
                    }
                }
            }

            if (!approvedNodes.Any())
            {
                throw new UiNodeNotFoundException(by);
            }

            return approvedNodes;
        }

        private bool CheckNode(XElement node, AttributeTags attributeTag, string value, bool equalTo, bool wildCard)
        {
            value = value.ToLower();
            switch (attributeTag)
            {
                case AttributeTags.TextContains:
                    return CheckNodeContainsText(node, value, equalTo);
                case AttributeTags.Text:
                    return CheckNodeAttribute(node, attributeTag, value, equalTo, wildCard);
                default:
                    throw new ArgumentOutOfRangeException(nameof(attributeTag), attributeTag, null);
            }
        }

        private bool CheckNodeContainsText(XElement node, string value, bool equalTo)
        {
            return node.Attribute("text").Value.ToLower().Contains(value) == equalTo;
        }

        private bool CheckNodeAttribute(XElement element, AttributeTags attributteTag, string value, bool equalTo, bool wildCard)
        {
            var nodeValue = string.Empty;
            switch (attributteTag)
            {
                case AttributeTags.Text:
                    nodeValue = element.Attribute("text").Value.ToLower();
                    break;
            }

            if (equalTo)
            {
                if (wildCard)
                {
                    return nodeValue.StartsWith(value);
                }

                return nodeValue == value;
            }

            if (wildCard)
            {
                return !nodeValue.StartsWith(value);
            }

            return nodeValue != value;
        }
    }
}
