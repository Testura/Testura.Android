using System;
using System.Collections.Generic;
using System.Data.Common;
using Testura.Android.Device;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Monkey
{
    public class ClickMonkey
    {
        public Random _rnd;

        public ClickMonkey()
        {
            _rnd = new Random();
        }

        public void Start(IAndroidDevice device, string package, string activity, IEnumerable<MonkeyCase> monkeyCases)
        {
            var d = new AndroidDevice();
            d.Activity.Start(package, activity, false, false);

            var o = device.Ui as UiService;
            while (true)
            {
                var nodes = o.GetAllNodesOnScreen();

                var selectedNode = nodes[_rnd.Next(0, nodes.Count)];

                foreach (var monkeyCase in monkeyCases)
                {
                    if (MatchCase(monkeyCase.Withs, selectedNode))
                    {
                        var shouldStillClick = monkeyCase.Case(d, selectedNode);
                        if (shouldStillClick)
                        {
                            device.Interaction.Tap(selectedNode);
                        }
                    }
                }
            }
        }

        public bool MatchCase(IList<With> withs, Node node)
        {
            foreach (var with in withs)
            {
                if (!with.NodeSearch(node))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
