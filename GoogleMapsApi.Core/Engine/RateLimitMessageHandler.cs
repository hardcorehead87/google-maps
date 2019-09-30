using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Core.Engine
{
    internal class RateLimitMessageHandler : DelegatingHandler
    {
        private readonly TimeSpan _limitTime;
        private readonly SemaphoreSlim _semaphore;

        public RateLimitMessageHandler(int limitCount, TimeSpan limitTime)
        {
            InnerHandler = new HttpClientHandler();
            _limitTime = limitTime;
            _semaphore = new SemaphoreSlim(limitCount);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            var result = await base.SendAsync(request, cancellationToken);
            DelayedRelease(_limitTime);
            return result;
        }
        private async Task DelayedRelease(TimeSpan delay)
        {
            await Task.Delay(delay);
            _semaphore.Release();
        }
    }
}