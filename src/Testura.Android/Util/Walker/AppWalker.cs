using System;
using System.Collections.Generic;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker
{
    public class AppWalker
    {
        private readonly AppWalkerConfiguration _appWalkerConfiguration;
        private Random _rnd;

        public AppWalker(AppWalkerConfiguration appWalkerConfiguration)
        {
            _appWalkerConfiguration = appWalkerConfiguration;
            _rnd = new Random();
        }

        public void Start(IAndroidDevice device, string package, string activity, IEnumerable<WalkerTrigger> monkeyCases)
        {
            var d = new AndroidDevice();
            if (package != null && activity != null)
            {
                d.Activity.Start(package, activity, false, false);
            }

            var o = device.Ui as UiService;
            string currentPackageAndActivity = device.Activity.GetCurrent();
            int numberOfTimesOnPackageAndAcivity = 0;
            while (true)
            {
                var nodes = o.GetAllNodesOnScreen();

                if (_appWalkerConfiguration.ShouldOnlyTapClickAbleNodes)
                {
                    nodes = nodes.Where(n => n.Clickable).ToList();
                }

                if (nodes.Count == 0)
                {
                    device.Interaction.InputKeyEvent(KeyEvents.Back);
                    continue;
                }

                var selectedNode = nodes[_rnd.Next(0, nodes.Count - 1)];

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

                device.Interaction.Tap(selectedNode);

                if (selectedNode.Focusable && selectedNode.LongClickable)
                {
                    device.Interaction.InputText("I HATE THIS");
                }

                var packageAndActivity = device.Activity.GetCurrent();

                if (!packageAndActivity.Contains(package))
                {
                    device.Activity.Start(package, activity, false, false);
                }

                if (packageAndActivity != currentPackageAndActivity)
                {
                    currentPackageAndActivity = packageAndActivity;
                    numberOfTimesOnPackageAndAcivity = 0;
                }
                else
                {
                    numberOfTimesOnPackageAndAcivity++;
                    if (numberOfTimesOnPackageAndAcivity == _appWalkerConfiguration.MaxInputBeforeGoingBack)
                    {
                        device.Interaction.InputKeyEvent(KeyEvents.Back);
                        numberOfTimesOnPackageAndAcivity = 0;
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
