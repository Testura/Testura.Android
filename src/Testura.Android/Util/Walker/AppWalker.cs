using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Testura.Android.Device;
using Testura.Android.Device.Services.Default;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Walker.Cases;
using Testura.Android.Util.Walker.Input;

namespace Testura.Android.Util.Walker
{
    public class AppWalker
    {
        private readonly IList<IAppWalkerInput> _inputs;
        private readonly AppWalkerConfiguration _appWalkerConfiguration;
        private readonly Random _rnd;

        public AppWalker(AppWalkerConfiguration appWalkerConfiguration, IList<IAppWalkerInput> inputs)
        {
            if (appWalkerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(appWalkerConfiguration));
            }

            _inputs = inputs ?? new List<IAppWalkerInput> {new TapAppWalkerInput()};
            _appWalkerConfiguration = appWalkerConfiguration;
            _rnd = new Random();
        }

        public AppWalker(AppWalkerConfiguration appWalkerConfiguration)
            : this(appWalkerConfiguration, null)
        {
        }

        /// <summary>
        /// Start a new app walker run
        /// </summary>
        /// <param name="device">Device to run walk with</param>
        /// <param name="package">Package to app walk. If null we start from current screen</param>
        /// <param name="activity">Activity to app walk. If null we start from current screen</param>
        /// <param name="tapCases">Tap cases with specific nodes that we want to handle</param>
        /// <param name="timeCases">Time cases with specific actions that we want to perform in intervals</param>
        /// <param name="stopCases">Stop cases to decide if we should stop the run</param>
        public void Start(IAndroidDevice device, string package, string activity, IEnumerable<TapCase> tapCases = null, IEnumerable<TimeCase> timeCases = null, IEnumerable<StopCase> stopCases = null)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (tapCases == null)
            {
                tapCases = new List<TapCase>();
            }

            if (timeCases == null)
            {
                timeCases = new List<TimeCase>();
            }

            if (stopCases == null)
            {
                stopCases = new List<StopCase>();
            }

            if (package != null && activity != null && _appWalkerConfiguration.ShouldStartActivity)
            {
                device.Activity.Start(package, activity, false, false);
            }

            foreach (var timeCase in timeCases)
            {
                timeCase.StartTimer(device);
            }

            var currentPackageAndActivity = device.Activity.GetCurrent();
            var numberOfTimesOnPackageAndAcivity = 0;
            var uiService = device.Ui as UiService;
            var start = DateTime.Now;

            while (true)
            {
                if (_appWalkerConfiguration.WalkDuration > 0)
                {
                    if ((DateTime.Now - start).Minutes > _appWalkerConfiguration.WalkDuration)
                    {
                        return;
                    }
                }

                var nodes = ForceGetAllNodes(uiService);

                if (StopWalk(device, stopCases, nodes))
                {
                    return;
                }

                _inputs[_rnd.Next(0, _inputs.Count)].PerformInput(device, tapCases, nodes, _appWalkerConfiguration);

                var packageAndActivity = device.Activity.GetCurrent();

                if (package != null && !packageAndActivity.Contains(package) && _appWalkerConfiguration.ShouldGoBackToActivity)
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

                Thread.Sleep(_appWalkerConfiguration.InputCooldown * 1000);
            }
        }

        /// <summary>
        /// Check if we have any case that want to stop the walk
        /// </summary>
        /// <param name="device">The current android device</param>
        /// <param name="stopCases">A list of provided stop cases</param>
        /// <param name="nodes">All nodes on the screen</param>
        /// <returns>True if we should stop the walk, false otherwise</returns>
        private bool StopWalk(IAndroidDevice device, IEnumerable<StopCase> stopCases, IList<Node> nodes)
        {
            var stopCase = stopCases.FirstOrDefault(s => s.IsMatching(nodes));
            if (stopCase != null)
            {
                return stopCase.Case.Invoke(device);
            }

            return false;
        }

        /// <summary>
        /// A method to force get all nodes as we sometimes get into a state where the root is missing.
        /// </summary>
        /// <param name="uiService">The ui service on the current device</param>
        /// <returns>All nodes on the screen</returns>
        private IList<Node> ForceGetAllNodes(UiService uiService)
        {
            var maxTries = 20;
            var tries = 0;
            while (true)
            {
                try
                {
                    return uiService.GetAllNodesOnScreen();
                }
                catch (UiNodeNotFoundException)
                {
                    if (tries >= maxTries)
                    {
                        throw;
                    }

                    tries++;
                }
            }
        }
    }
}
