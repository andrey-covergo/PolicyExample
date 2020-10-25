using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyExample.API.Web.Controllers;
using PolicyExampleAPI;

namespace PolicyExample.API.Web
{
    public class Startup
    {
        private bool _isDevelopment;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var generatedAssembly = typeof(PolicyController).Assembly;
            services
                .AddControllers()
                .AddApplicationPart(generatedAssembly)
                .AddControllersAsServices();

            services.AddTransient<IConfigurationController,ConfigurationControllerLogic>();
            services.AddTransient<IClaimsController, ClaimsControllerLogic>();
            services.AddTransient<IIssuanceController, IssuanceControllerLogic>();
            services.AddTransient<IPolicyController, PolicyControllerLogic>();
            services.AddTransient<IBusinessTimeController, BusinessTimeControllerLogic>();

            if (_isDevelopment)
            {
                services.AddCors(o => o.AddPolicy("Enable CORS for everybody", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                _isDevelopment = true;
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapControllers();
            });
        }
    }
}