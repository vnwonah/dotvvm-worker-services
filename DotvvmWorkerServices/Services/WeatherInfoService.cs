using DotvvmWorkerServices.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotvvmWorkerServices.Services
{
    public class WeatherInfoService
    {
        private readonly HttpClient _client;
        private ILogger<WeatherInfoService> _logger;
        public WeatherInfoService(
            HttpClient client,
            ILogger<WeatherInfoService> logger)
        {
            _client = client;
            _logger = logger;

        }

        public async Task<WeatherInfo> GetWeatherData(string @param)
        {
            try
            {
                var response = await _client.GetAsync($"/weather?units=metric&q={param}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<WeatherInfo>(responseString);
                return responseModel;
            }
            catch (Exception e)
            {
                _logger.LogError("error on api call", e);
                return null;
            }
        }
    }
}
