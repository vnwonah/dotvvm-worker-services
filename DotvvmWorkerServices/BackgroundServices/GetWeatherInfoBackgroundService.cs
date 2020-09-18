using DotvvmWorkerServices.Models;
using DotvvmWorkerServices.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotvvmWorkerServices.BackgroundServices
{
    public class GetWeatherInfoBackgroundService : BackgroundService
    {
        private readonly IMemoryCache _cache;
        private readonly WeatherInfoService _weatherInfoService;
        public GetWeatherInfoBackgroundService(
            IMemoryCache cache,
            WeatherInfoService weatherInfoService)
        {
            _cache = cache;
            _weatherInfoService = weatherInfoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cache.Set<List<long>>(AppConfig.WEATHER_INFOS_KEY, new List<long>());
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var weatherInfo = await _weatherInfoService.GetWeatherData("Lagos,Nigeria");
                    var values = _cache.Get<List<long>>(AppConfig.WEATHER_INFOS_KEY);
                    if (values.Count > 9)
                    {
                        var oldest = values.Min();
                        values.Remove(oldest);
                        _cache.Remove(oldest);
                    }
                    values.Add(weatherInfo.Dt.Value);
                    _cache.Set<List<long>>(AppConfig.WEATHER_INFOS_KEY, values);
                    _cache.Set<WeatherInfo>(weatherInfo.Dt.Value, weatherInfo);
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                catch (Exception e)
                {
                }
                
            }
        }
    }
}
