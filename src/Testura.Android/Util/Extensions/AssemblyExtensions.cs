using System;
using System.IO;
using System.Reflection;
#pragma warning disable 1591

namespace Testura.Android.Util.Extensions
{
    internal static class AssemblyExtensions
    {
        internal static string GetDirectoryPath(this Assembly assembly)
        {
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
