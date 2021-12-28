﻿using Testura.Android.Device;
using Testura.Android.Device.Ui.Nodes.Data;

namespace Testura.Android.Util.Walker.Input
{
    /// <summary>
    /// Provides functionality to send swipe input when app walking.
    /// </summary>
    public class SwipeAppWalkerInput : IAppWalkerInput
    {
        private readonly Random _rnd;
        private readonly IList<SwipeDirection> _swipeDirectionses;
        private readonly TimeSpan _duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeAppWalkerInput"/> class.
        /// </summary>
        /// <param name="swipeDirectionses">A set of possible swipe directions.</param>
        /// <param name="duration">The swipe duration.</param>
        public SwipeAppWalkerInput(IList<SwipeDirection> swipeDirectionses, TimeSpan duration)
        {
            _rnd = new Random();
            _swipeDirectionses = swipeDirectionses;
            _duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeAppWalkerInput"/> class.
        /// </summary>
        /// <param name="duration">The swipe duration in milliseconds.</param>
        public SwipeAppWalkerInput(TimeSpan duration)
        {
            _duration = duration;
            _rnd = new Random();
            _swipeDirectionses = new List<SwipeDirection>
            {
                SwipeDirection.Down,
                SwipeDirection.Left,
                SwipeDirection.Right,
                SwipeDirection.Up
            };
        }

        /// <summary>
        /// Perform app walker input
        /// </summary>
        /// <param name="device">The current device</param>
        /// <param name="nodes">All nodes on current screen</param>
        public void PerformInput(IAndroidDevice device, IList<Node> nodes)
        {
            device.Interaction.Swipe(_swipeDirectionses[_rnd.Next(0, _swipeDirectionses.Count)], _duration);
        }
    }
}
