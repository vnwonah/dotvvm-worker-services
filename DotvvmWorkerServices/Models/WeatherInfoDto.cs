using System;

namespace DotvvmWorkerServices.Models
{
    public class WeatherInfoDto
    {
        public DateTime DateTime { get; set; }
        public string Weather { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
}
