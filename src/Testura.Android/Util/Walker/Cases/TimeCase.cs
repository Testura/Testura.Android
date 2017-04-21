using System;
using System.Timers;
using Testura.Android.Device;

namespace Testura.Android.Util.Walker.Cases
{
    public class TimeCase
    {
        private readonly Action<IAndroidDevice> _timeCase;
        private readonly Timer _timer;
        private IAndroidDevice _device;

        public TimeCase(double interval, Action<IAndroidDevice> timeCase)
        {
            _timeCase = timeCase;
            _timer = new Timer(interval);
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        /// <param name="device">The current device</param>
        public void StartTimer(IAndroidDevice device)
        {
            _timer.Start();
            _timer.Elapsed += TimerOnElapsed;
            _device = device;
        }

        /// <summary>
        /// Peform time case action
        /// </summary>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timeCase.Invoke(_device);
        }
    }
}
