using System;
using System.Collections.Generic;

namespace Testura.Android.Util.Recording
{
    /// <summary>
    /// Contains all available configurations for a recording
    /// </summary>
    public class ScreenRecordConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecordConfiguration"/> class.
        /// </summary>
        public ScreenRecordConfiguration()
        {
            TimeLimit = TimeSpan.FromMinutes(3);
            Size = null;
            BitRate = 4000000;
        }

        /// <summary>
        /// Gets or sets the maximum recording time, in seconds. The default and maximum value is 180 (3 minutes).
        /// </summary>
        public TimeSpan TimeLimit { get; set; }

        /// <summary>
        /// Gets or sets the video size: 1280x720. The default value is the device's native display resolution (if supported), 1280x720 if not. For best results, use a size supported by your device's Advanced Video Coding (AVC) encoder.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the video bit rate for the video, in megabits per second. The default value is 4Mbps. You can increase the bit rate to improve video quality, but doing so results in larger movie files. The following example sets the recording bit rate to 6Mbps: 6000000
        /// </summary>
        public int BitRate { get; set; }

        internal string[] GetArguments()
        {
            var arguments = new List<string>
            {
                "--time-limit",
                TimeLimit.TotalSeconds.ToString(),
                "--bit-rate",
                BitRate.ToString()
            };

            if (!string.IsNullOrEmpty(Size))
            {
                arguments.Add("--size");
                arguments.Add(Size);
            }

            return arguments.ToArray();
        }
    }
}
