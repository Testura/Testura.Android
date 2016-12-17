﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Testura.Android.Util.Exceptions;

namespace Testura.Android.Device.Services.Default
{
    public class ActivityService : Service, IActivityService
    {
        /// <summary>
        /// Start an Activity specified by package and activity name
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <param name="activity">Name of the activity</param>
        /// <param name="forceStopActivity">Force stop the target app before starting the activity.</param>
        /// <param name="clearTasks">Clear activity stack</param>
        /// <exception cref="AdbException">Thrown if we can't find package/activity</exception>
        public void Start(string packageName, string activity, bool forceStopActivity, bool clearTasks)
        {
            if (string.IsNullOrEmpty(packageName))
            {
                throw new ArgumentException("Argument is null or empty", nameof(packageName));
            }

            if (string.IsNullOrWhiteSpace(activity))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(activity));
            }

            var commandBuilder = new StringBuilder($"am start -W -n {packageName}/{activity}");
            if (forceStopActivity)
            {
                commandBuilder.Append(" -S");
            }

            if (clearTasks)
            {
                commandBuilder.Append(" --activity-clear-task");
            }

            var result = Device.Adb.Shell(commandBuilder.ToString());
            if (result.Contains("Error") || result.Contains("does not exist") || result.Contains("Exception"))
            {
                throw new AdbException(result);
            }
        }

        /// <summary>
        /// Get the current open acitivity
        /// </summary>
        /// <returns>Current open activity</returns>
        public string GetCurrent()
        {
            var activity = Device.Adb.Shell("dumpsys window windows | grep -E 'mCurrentFocus|mFocusedApp'");
            var regex = new Regex(@"(?<=\{)[^}]*(?=\})");
            var matches = regex.Matches(activity);
            if (matches.Count == 0)
            {
                return "Unkown activity";
            }

            return matches[0].Value.Split(' ').Last();
        }
    }
}
