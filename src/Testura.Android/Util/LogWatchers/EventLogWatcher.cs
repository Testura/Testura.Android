using System;
using Testura.Android.Device.Configurations;

namespace Testura.Android.Util.LogWatchers
{
    public class EventLogWatcher : LogWatcher
    {
        public EventLogWatcher(DeviceConfiguration deviceConfiguration, string[] tags, bool flushLogcat = false)
            : base(deviceConfiguration, tags, flushLogcat)
        {
        }

        /// <summary>
        /// NewOutputEvent is raised every time we receive another matching line from logcat
        /// </summary>
        public event EventHandler<string> NewOutputEvent;

        /// <summary>
        /// Called when we receive another matching line from logcat
        /// </summary>
        /// <param name="output">Last matching line from logcat</param>
        protected override void NewOutput(string output)
        {
            NewOutputEvent?.Invoke(this, output);
        }
    }
}
