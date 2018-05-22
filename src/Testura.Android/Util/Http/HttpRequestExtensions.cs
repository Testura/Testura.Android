using System;
using System.Net.Http;

namespace Testura.Android.Util.Http
{
    internal static class HttpRequestExtensions
    {
        private const string TimeoutPropertyKey = "RequestTimeout";

        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Properties[TimeoutPropertyKey] = timeout;
        }

        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            object value;

            if (request.Properties.TryGetValue(TimeoutPropertyKey, out value))
            {
                if (value is TimeSpan)
                {
                    return value as TimeSpan?;
                }
            }

            return null;
        }
    }
}
