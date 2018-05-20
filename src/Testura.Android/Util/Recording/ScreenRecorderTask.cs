using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Testura.Android.Device;
using Testura.Android.Util.Logging;
using Testura.Android.Util.Terminal;

namespace Testura.Android.Util.Recording
{
    /// <summary>
    /// Helper class for screen recording.
    /// </summary>
    public class ScreenRecorderTask
    {
        private readonly IAndroidDevice _device;
        private readonly string _temporaryDeviceDirectory;
        private readonly ITerminal _terminal;
        private string _lastFullTemporaryRecordingPath;

        internal ScreenRecorderTask(IAndroidDevice device, string temporaryDeviceDirectory)
        {
            _device = device;
            _temporaryDeviceDirectory = temporaryDeviceDirectory;
            _terminal = new Terminal.Terminal(device.Configuration);
        }

        /// <summary>
        /// Stop a screen recording and pull the created recording to local computer.
        /// </summary>
        /// <param name="savePath">Local path for saving</param>
        /// <param name="removeRecordingFromDevice">True if we should remove recording from device after pulling.</param>
        public void StopRecording(string savePath, bool removeRecordingFromDevice = true)
        {
            DeviceLogger.Log("Request to stop recording..");
            StopRecordingProcess();
            DeviceLogger.Log($"Pulling recording to {savePath}");
            _device.Adb.Pull(_lastFullTemporaryRecordingPath, savePath);
            RemoveTemporaryRecordingOnDevice(removeRecordingFromDevice);
        }

        /// <summary>
        /// Stop screen recordings without pulling any.
        /// </summary>
        /// <param name="removeRecordingFromDevice">True if we should remove recording from device after pulling.</param>
        public void StopRecording(bool removeRecordingFromDevice = true)
        {
            DeviceLogger.Log("Request to stop recording without saving movie..");
            StopRecordingProcess();
            RemoveTemporaryRecordingOnDevice(removeRecordingFromDevice);
        }

        internal void StartRecording(ScreenRecordConfiguration configurations)
        {
            if (configurations == null)
            {
                throw new ArgumentNullException(nameof(configurations));
            }

            TerminateAllCurrentRecordings();
            DeviceLogger.Log($"Starting new screen recording with {configurations.TimeLimit.TotalSeconds} seconds time limit...");
            _lastFullTemporaryRecordingPath = Path.Combine(_temporaryDeviceDirectory, $"{Guid.NewGuid().ToString()}.mp4");
            DeviceLogger.Log($"Saving temporary recording at {_lastFullTemporaryRecordingPath}");

            var commands = new List<string>
            {
                "shell",
                "screenrecord",
                _lastFullTemporaryRecordingPath,
            };
            commands.AddRange(configurations.GetArguments());
            _terminal.StartAdbProcessWithoutShell(commands.ToArray());
        }

        private void StopRecordingProcess()
        {
            TerminateAllCurrentRecordings();
        }

        private void RemoveTemporaryRecordingOnDevice(bool removeRecordingFromDevice)
        {
            if (removeRecordingFromDevice)
            {
                _device.Adb.Shell($"rm -f {_lastFullTemporaryRecordingPath}");
            }
        }

        private void TerminateAllCurrentRecordings()
        {
            DeviceLogger.Log("Terminating all current recordings..");

            try
            {
                var screenRecordings = _device.Adb.Shell("ps | grep screenrecord")
                    .Split(new[] {"\r\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var screenRecording in screenRecordings)
                {
                    var fixedRow = screenRecording.Replace("shell", string.Empty).Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                    if (fixedRow.Any())
                    {
                        var pid = fixedRow.First();
                        _device.Adb.Shell($"kill -2 {pid}");
                    }
                }
            }
            catch (Exception)
            {
                // No recordings found
            }
        }
    }
}