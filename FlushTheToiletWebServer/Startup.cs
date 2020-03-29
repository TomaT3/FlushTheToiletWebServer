using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlushTheToiletWebServer.CF01;
using FlushTheToiletWebServer.Models;
using FlushTheToiletWebServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace FlushTheToiletWebServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc();

            // start state machine
            var stateMachine = app.ApplicationServices.GetService<IToiletFlusherStateMachineModel>();
            stateMachine.StartToiletStateMachine();
        }

        private void RegisterCf01Modules(IServiceCollection services)
        {
            services.AddSingleton<IFlusherMotor, FlusherMotor>();
            services.AddSingleton<ILedControl, LedControl>();
            services.AddSingleton<IManDetectionDistanceSensor, ManDetectionDistanceSensor>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IFlushService, FlushService>();
            services.AddSingleton<IManDetectionService, ManDetectionService>();
            services.AddSingleton<IToiletFlusherStateMachine, ToiletFlusherStateMachine>();
        }

        private void RegisterModels(IServiceCollection services)
        {
            services.AddSingleton<IFlusherMotorModel, FlusherMotorModel>();
            services.AddSingleton<IManDetectionModel, ManDetectionModel>();
            services.AddSingleton<IToiletFlusherStateMachineModel, ToiletFlusherStateMachineModel>();
        }
    }
}
