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
using PolicyExample.API.Rest.Controllers;
using PolicyExampleAPI;

namespace PolicyExample.API.Rest
{
    public class Startup
    {
        private bool _isDevelopment;

        private const string OpenApiWebEditorCorsPolicyName = "AllowOpenAPIWebEditors";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(o => o.AddPolicy(OpenApiWebEditorCorsPolicyName, builder =>
            {
                builder.WithOrigins("https://covergo.stoplight.io",
                                    "https://editor.swagger.io")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            }));

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

          
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseOptions();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapControllers().RequireCors(OpenApiWebEditorCorsPolicyName);
            });
            
           
        }
    }
}