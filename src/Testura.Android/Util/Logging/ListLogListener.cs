using System;
using System.Collections.Generic;

namespace Testura.Android.Util.Logging
{
    public class ListLogListener : ILogListener
    {
        private readonly DeviceLogger.LogLevels _minLogLevel;
        private readonly List<string> _logMessages;

        public ListLogListener(DeviceLogger.LogLevels minLogLevel)
        {
            _minLogLevel = minLogLevel;
            _logMessages = new List<string>();
        }

        public IReadOnlyList<string> LogMessages => _logMessages;

        public void Log(string className, string memberName, string message, DeviceLogger.LogLevels logLevel)
        {
            if (logLevel < _minLogLevel)
            {
                return;
            }

            _logMessages.Add($"[{logLevel.ToString()}][{DateTime.Now}][{className}][{memberName}]: {message}");
        }
    }
}
