using System.Timers;
using Testura.Android.Device;

namespace Testura.Android.Util.Walker.Cases.Time
{
    /// <summary>
    /// Provides the base class from which classes that represent time cases in app walker are derived.
    /// </summary>
    public abstract class TimeCase
    {
        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCase"/> class.
        /// </summary>
        /// <param name="interval">Time between time action in milliseconds.</param>
        protected TimeCase(double interval)
        {
            _timer = new Timer(interval);
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
            _timer.Start();
            _timer.Elapsed += TimerOnElapsed;
            Device = device;
        }

        /// <summary>
        /// Execute the case.
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        /// Perform time case action.
        /// </summary>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Execute();
            _timer.Stop();
            _timer.Start();
        }
    }
}
