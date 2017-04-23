using System;
using Testura.Android.Device;

namespace Testura.Android.Util.Walker.Cases.Time
{
    public class FunTimeCase : TimeCase
    {
        private readonly Action<IAndroidDevice> _timeCase;

        public FunTimeCase(double interval, Action<IAndroidDevice> timeCase)
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
