using DotVVM.Framework.ViewModel;
using DotvvmWorkerServices.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotvvmWorkerServices.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<DefaultViewModel> _logger;


        [Bind(Direction.ServerToClient)]
        public List<WeatherInfoDto> WeatherInfos { get; set; }
        public DefaultViewModel(
            IMemoryCache cache,
            ILogger<DefaultViewModel> logger)
		{
            _cache = cache;
            _logger = logger;
            WeatherInfos = new List<WeatherInfoDto>();
		}

        public override Task PreRender()
        {
            var weatherInfoModels = new List<WeatherInfo>();
            var keys = _cache.Get<List<long>>(AppConfig.WEATHER_INFOS_KEY);
            foreach (var key in keys)
            {
                var wi = _cache.Get<WeatherInfo>(key);
                if(wi is object)
                    weatherInfoModels.Add(wi);
            }
            foreach (var wim in weatherInfoModels)
            {
                try
                {
                    if (!wim.Weather.Any()) continue;
                    var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(wim.Dt.Value).ToLocalTime();

                    var wid = new WeatherInfoDto
                    {
                        DateTime = dtDateTime,
                        Weather = wim.Weather[0].Main,
                        Description = wim.Weather[0].Description,
                        Temperature = wim.Main.Temp.Value,
                        Pressure = wim.Main.Pressure.Value,
                        Humidity = wim.Main.Humidity.Value
                    };

                    WeatherInfos.Add(wid);
                    WeatherInfos = WeatherInfos.OrderByDescending(wis => wis.DateTime).ToList();
                }
                catch (Exception e)
                {
                    _logger.LogError("error occured while adding wim", e);
                }
            }
            return base.PreRender();
        }
    }
}
