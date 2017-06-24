using System;
using Testura.Android.Device;

namespace Testura.Android.Util.Walker.Cases.Time
{
    /// <summary>
    /// Provides the functionality to invoke an action after a specific interval
    /// </summary>
    public class ActionTimeCase : TimeCase
    {
        private readonly Action<IAndroidDevice> _timeCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTimeCase"/> class.
        /// </summary>
        /// <param name="interval">Time between each second in milliseconds</param>
        /// <param name="timeCase">Action to invoke</param>
        public ActionTimeCase(double interval, Action<IAndroidDevice> timeCase)
            : base(interval)
        {
            _timeCase = timeCase;
        }

        /// <summary>
        /// Execute the case
        /// </summary>
        protected override void Execute()
        {
            _timeCase.Invoke(Device);
        }
    }
}
