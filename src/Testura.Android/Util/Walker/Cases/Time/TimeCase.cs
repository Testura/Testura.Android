using System;
using System.Timers;
using Testura.Android.Device;

namespace Testura.Android.Util.Walker.Cases.Time
{
    /// <summary>
    /// Provides the base class from which classes that represent time cases in app walker are derived.
    /// </summary>
    public abstract class TimeCase
    {
        private readonly TimeSpan _interval;
        private DateTime _start;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCase"/> class.
        /// </summary>
        /// <param name="interval">Time between time action</param>
        protected TimeCase(TimeSpan interval)
        {
            _interval = interval;
        }

        /// <summary>
        /// Gets the android device.
        /// </summary>
        protected IAndroidDevice Device { get; private set; }

        /// <summary>
        /// Start the timer.
        /// </summary>
        /// <param name="device">The current device.</param>
        public void StartTimer(IAndroidDevice device)
        {
            _start = DateTime.Now;
            Device = device;
        }

        /// <summary>
        /// Execute the case.
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        /// Check timer
        /// </summary>
        public void CheckTimer()
        {
            if ((DateTime.Now - _start).TotalMinutes > _interval.TotalMinutes)
            {
                Execute();
                _start = DateTime.Now;
            }
        }
    }
}
