using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Testura.Android.Util.Http
{
    internal class TimeoutHandler : DelegatingHandler
    {
        public TimeoutHandler(TimeSpan defaultTimeout, HttpClientHandler innerHandler)
        {
            DefaultTimeout = defaultTimeout;
            InnerHandler = innerHandler;
        }

        public TimeSpan DefaultTimeout { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var cts = GetCancellationTokenSource(request, cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        private CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout() ?? DefaultTimeout;
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                // No need to create a CTS if there's no timeout
                return null;
            }

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);
            return cts;
        }
    }
}