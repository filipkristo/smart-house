using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHouseCore;
using SmartHouseDataStore;
using SmartHouseDevice;
using SmartHouseCommon;
using System.Net;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;

namespace SmartHouseGatewayApp
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
            services.Configure<ForwardedHeadersOptions>(options => options.KnownProxies.Add(IPAddress.Any));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.ConfireSmartHouseCoreServices();
            services.ConfireSmartHouseDataStoreServices();
            services.ConfireSmartHouseDeviceServices();
            services.ConfireSmartHouseCommonServices();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SmartHouse Gateway",
                    Description = "SmartHouse Gateway",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "Filip Krišto",
                        Email = "filipkristo@windowslive.com"
                    }
                });
            });

            services.AddHealthChecks();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHealthChecks("/health");
            loggerFactory.AddSerilog();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartHouse Gateway API V1");
                c.DisplayRequestDuration();
            });
        }
    }
}
