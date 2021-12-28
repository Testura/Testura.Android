using System.Reflection;
#pragma warning disable 1591

namespace Testura.Android.Util.Extensions
{
    internal static class AssemblyExtensions
    {
        internal static string GetDirectoryPath(this Assembly assembly)
        {
            var codeBase = assembly.Location;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
