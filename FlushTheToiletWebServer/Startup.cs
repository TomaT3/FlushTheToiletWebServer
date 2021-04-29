using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;
using FlushTheToiletWebServer.CF01;
using FlushTheToiletWebServer.Models;
using FlushTheToiletWebServer.Services;
using ioBroker.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NSubstitute;

namespace FlushTheToiletWebServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddControllers();

            RegisterCf01Modules(services);
            RegisterServices(services);
            RegisterModels(services);


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flush The Toilet Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flush The Toilet Api V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // start state machine
            var stateMachine = app.ApplicationServices.GetService<IToiletFlusherStateMachineModel>();
            stateMachine.StartToiletStateMachine();
        }

        private void RegisterCf01Modules(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                services.AddSingleton<IGpioController>(_ => Substitute.For<IGpioController>());
            }
            else
            {
                services.AddSingleton<IGpioController, GpioController>();
            }
            services.AddSingleton<IFlusherMotor, FlusherMotor>();
            services.AddSingleton<ILedControl, LedControl>();
            services.AddSingleton<IManDetectionDistanceSensor, ManDetectionDistanceSensor>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IFlushService, FlushService>();
            services.AddSingleton<IManDetectionService, ManDetectionService>();
            services.AddSingleton<IToiletFlusherStateMachine, ToiletFlusherStateMachine>();
            services.AddSingleton<IIoBrokerDotNet>(_ => new IoBrokerDotNet(@"http://iobroker:8084"));
        }

        private void RegisterModels(IServiceCollection services)
        {
            services.AddSingleton<IFlusherMotorModel, FlusherMotorModel>();
            services.AddSingleton<IManDetectionModel, ManDetectionModel>();
            services.AddSingleton<IToiletFlusherStateMachineModel, ToiletFlusherStateMachineModel>();
        }
    }
}
