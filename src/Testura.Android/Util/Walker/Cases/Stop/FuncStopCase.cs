using System;
using System.Collections.Generic;
using Testura.Android.Device;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util.Walker.Cases.Stop
{
    /// <summary>
    /// Provides functionality to stop am app walker run from a func.
    /// </summary>
    public class FuncStopCase : StopCase
    {
        private readonly Func<IAndroidDevice, bool> _case;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncStopCase"/> class.
        /// </summary>
        /// <param name="wheres">"Where(s)" used to find the node in our case</param>
        /// <param name="case">A func which take the current device and return if we should stop the run or not</param>
        public FuncStopCase(IList<Where> wheres, Func<IAndroidDevice, bool> @case)
            : base(wheres)
        {
            _case = @case;
        }

        /// <summary>
        /// Execute the case
        /// </summary>
        /// <param name="device">The current device</param>
        /// <returns>True if we should stop the app walker run, false otherwise</returns>
        public override bool Execute(IAndroidDevice device)
        {
            return _case.Invoke(device);
        }
    }
}
