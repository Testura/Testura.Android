using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medallion.Shell;
using Testura.Android.Device.Services.Adb;
using Testura.Android.Util.Logging;

namespace Testura.Android.Util.Recording
{
    /// <summary>
    /// Helper class for screen recording.
    /// </summary>
    public class ScreenRecorderTask
    {
        private readonly AdbService _adbService;
        private readonly AdbTerminal _adbTerminal;
        private readonly string _temporaryDeviceDirectory;
        private string _lastFullTemporaryRecordingPath;

        internal ScreenRecorderTask(AdbService adbService, AdbTerminal adbTerminal, string temporaryDeviceDirectory)
        {
            _adbService = adbService;
            _adbTerminal = adbTerminal;
            _temporaryDeviceDirectory = temporaryDeviceDirectory;
        }

        /// <summary>
        /// Stop a screen recording and pull the created recording to local computer.
        /// </summary>
        /// <param name="savePath">Local path for saving</param>
        /// <param name="removeRecordingFromDevice">True if we should remove recording from device after pulling.</param>
        public void StopRecording(string savePath, bool removeRecordingFromDevice = true)
        {
            DeviceLogger.Log("Request to stop recording..", DeviceLogger.LogLevels.Info);
            StopRecordingProcess();
            DeviceLogger.Log($"Pulling recording to {savePath}", DeviceLogger.LogLevels.Info);
            _adbService.Pull(_lastFullTemporaryRecordingPath, savePath);
            RemoveTemporaryRecordingOnDevice(removeRecordingFromDevice);
        }

        /// <summary>
        /// Stop screen recordings without pulling any.
        /// </summary>
        /// <param name="removeRecordingFromDevice">True if we should remove recording from device after pulling.</param>
        public void StopRecording(bool removeRecordingFromDevice = true)
        {
            DeviceLogger.Log("Request to stop recording without saving movie..", DeviceLogger.LogLevels.Info);
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
            DeviceLogger.Log($"Starting new screen recording with {configurations.TimeLimit.TotalSeconds} seconds time limit...", DeviceLogger.LogLevels.Info);
            _lastFullTemporaryRecordingPath = Path.Combine(_temporaryDeviceDirectory, $"{Guid.NewGuid().ToString()}.mp4");
            DeviceLogger.Log($"Saving temporary recording at {_lastFullTemporaryRecordingPath}", DeviceLogger.LogLevels.Info);

            var commands = new List<string>
            {
                "shell",
                "screenrecord",
                _lastFullTemporaryRecordingPath,
            };
            commands.AddRange(configurations.GetArguments());
            _adbTerminal.StartAdbProcessWithoutShell(commands.ToArray());
        }

        private void StopRecordingProcess()
        {
            TerminateAllCurrentRecordings();
        }

        private void RemoveTemporaryRecordingOnDevice(bool removeRecordingFromDevice)
        {
            if (removeRecordingFromDevice)
            {
                _adbService.Shell($"rm -f {_lastFullTemporaryRecordingPath}");
            }
        }

        private void TerminateAllCurrentRecordings()
        {
            DeviceLogger.Log("Terminating all current recordings..", DeviceLogger.LogLevels.Info);

            try
            {
                var screenRecordings = GetCurrentRecordingProcesses();
                foreach (var screenRecording in screenRecordings)
                {
                    var fixedRow = screenRecording.Replace("shell", string.Empty)
                        .Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                    if (fixedRow.Any())
                    {
                        var pid = fixedRow.First();
                        _adbService.Shell($"kill -2 {pid}");
                    }
                }

                var start = DateTime.Now;
                while ((DateTime.Now - start).Minutes < 2 && GetCurrentRecordingProcesses().Any())
                {
                }

                DeviceLogger.Log("Could not terminate recording process?", DeviceLogger.LogLevels.Warning);
            }
            catch (Exception)
            {
                // No recordings found
            }
        }

        private string[] GetCurrentRecordingProcesses()
        {
            return _adbService.Shell("ps | grep screenrecord").Split(new[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}