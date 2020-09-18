using DotVVM.Framework.Routing;
using DotvvmWorkerServices.BackgroundServices;
using DotvvmWorkerServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace DotvvmWorkerServices
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<GetWeatherInfoBackgroundService>();
            services.AddHttpClient<WeatherInfoService>(client =>
            {
                client.BaseAddress = new Uri("https://community-open-weather-map.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "community-open-weather-map.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "YOUR - KEY - HERE");
            });
            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();
            services.AddDotVVM<DotvvmStartup>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
            dotvvmConfiguration.AssertConfigurationIsValid();
            
            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath)
            });
        }
    }
}
