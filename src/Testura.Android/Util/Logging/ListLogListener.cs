using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Testura.Android.Util.Logging
{
    public class ListLogListener : ILogListener
    {
        private List<string> _logMessages;

        public ListLogListener()
        {
            _logMessages = new List<string>();
        }

        public IReadOnlyList<string> LogMessages => _logMessages;

        public void Log(string className, string memberName, string message)
        {
            _logMessages.Add($"[{DateTime.Now}][{className}][{memberName}]: {message}");
        }
    }
}
