using System;
using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;
using Testura.Android.Util.Walker.Cases;

namespace Testura.Android.Util.Walker.Input
{
    public class SwipeAppWalkerInput : IAppWalkerInput
    {
        private readonly Random _rnd;
        private readonly IList<SwipeDirections> _swipeDirectionses;
        private readonly int _duration;

        public SwipeAppWalkerInput(IList<SwipeDirections> swipeDirectionses, int duration)
        {
            _rnd = new Random();
            _swipeDirectionses = swipeDirectionses;
            _duration = duration;
        }

        public SwipeAppWalkerInput(int duration)
        {
            _duration = duration;
            _rnd = new Random();
            _swipeDirectionses = new List<SwipeDirections>
            {
                SwipeDirections.Down,
                SwipeDirections.Left,
                SwipeDirections.Right,
                SwipeDirections.Up
            };
        }

        /// <summary>
        /// Peform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="tapCases">List of provided tap cases</param>
        /// <param name="nodes">All nodes on current screen</param>
        public void PerformInput(IAndroidDevice device, IEnumerable<TapCase> tapCases, IList<Node> nodes)
        {
            device.Interaction.Swipe(_swipeDirectionses[_rnd.Next(0, _swipeDirectionses.Count)], _duration);
        }
    }
}
