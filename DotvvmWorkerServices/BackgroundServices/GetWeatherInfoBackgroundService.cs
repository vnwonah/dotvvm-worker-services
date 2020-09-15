using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotvvmWorkerServices.BackgroundServices
{
    public class GetWeatherInfoBackgroundService : BackgroundService
    {
        private readonly IMemoryCache _cache;
        public GetWeatherInfoBackgroundService(IMemoryCache cache)
        {
            _cache = cache;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
