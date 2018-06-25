using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Testura.Android.Device;
using Testura.Android.Device.Services.Ui;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util.Exceptions;
using Testura.Android.Util.Walker.Cases.Stop;
using Testura.Android.Util.Walker.Cases.Time;
using Testura.Android.Util.Walker.Input;

namespace Testura.Android.Util.Walker
{
    /// <summary>
    /// Provides the functionality to walk through an application and perform different actions.
    /// </summary>
    public class AppWalker
    {
        private const int ForceMaxTries = 20;
        private readonly IList<IAppWalkerInput> _inputs;
        private readonly AppWalkerConfiguration _appWalkerConfiguration;
        private readonly Random _rnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppWalker"/> class.
        /// </summary>
        /// <param name="appWalkerConfiguration">The configuration that describes how we should run the walker.</param>
        /// <param name="inputs">A set of allowed inputs.</param>
        public AppWalker(AppWalkerConfiguration appWalkerConfiguration, IList<IAppWalkerInput> inputs)
        {
            _inputs = inputs ?? new List<IAppWalkerInput> { new TapAppWalkerInput(true) };
            _appWalkerConfiguration = appWalkerConfiguration ?? throw new ArgumentNullException(nameof(appWalkerConfiguration));
            _rnd = new Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppWalker"/> class.
        /// </summary>
        /// <param name="appWalkerConfiguration">The configuration that describes how we should run the walker.</param>
        public AppWalker(AppWalkerConfiguration appWalkerConfiguration)
            : this(appWalkerConfiguration, null)
        {
        }

        /// <summary>
        /// Start the app walker
        /// </summary>
        /// <param name="device">Device to run walk with</param>
        /// <param name="package">Package to app walk. If null we start from current screen</param>
        /// <param name="activity">Activity to app walk. If null we start from current screen</param>
        public void Start(IAndroidDevice device, string package, string activity)
        {
            Start(device, package, activity, new List<TimeCase>(), new List<StopCase>());
        }

        /// <summary>
        /// Start the app walker
        /// </summary>
        /// <param name="device">Device to run walk with</param>
        /// <param name="package">Package to app walk. If null we start from current screen</param>
        /// <param name="activity">Activity to app walk. If null we start from current screen</param>
        /// <param name="timeCases">Time cases with specific actions that we want to perform in intervals</param>
        /// <param name="stopCases">Stop cases to decide if we should stop the run</param>
        public void Start(IAndroidDevice device, string package, string activity, IEnumerable<TimeCase> timeCases, IEnumerable<StopCase> stopCases)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (timeCases == null)
            {
                timeCases = new List<TimeCase>();
            }

            if (stopCases == null)
            {
                stopCases = new List<FuncStopCase>();
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
            var uiService = device.Ui;
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

                _inputs[_rnd.Next(0, _inputs.Count)].PerformInput(device, nodes);

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
                        device.Interaction.InputKeyEvent(KeyEvent.Back);
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
                return stopCase.Execute(device);
            }

            return false;
        }

        /// <summary>
        /// A method to force get all nodes as we sometimes get into a state where the root is missing.
        /// </summary>
        /// <param name="uiService">The ui service on the current device</param>
        /// <returns>All nodes on the screen</returns>
        private IList<Node> ForceGetAllNodes(INodeFinderService uiService)
        {
            var tries = 0;
            while (true)
            {
                try
                {
                    return uiService.AllNodes();
                }
                catch (UiNodeNotFoundException)
                {
                    if (tries >= ForceMaxTries)
                    {
                        throw;
                    }

                    tries++;
                }
            }
        }
    }
}
