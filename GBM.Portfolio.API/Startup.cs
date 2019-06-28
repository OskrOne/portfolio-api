using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBM.Portfolio.Domain.Repositories;
using GBM.Portfolio.Domain.Repositories.Events;
using GBM.Portfolio.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GBM.Portfolio.DataAccess
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
            var repositoryConfig = new RepositoryConfig()
            {
                Local = bool.Parse(Configuration["AWS:Local"]),
                DynamoDBURL = Configuration["AWS:DynamoDBURL"],
                AwsAccessKeyId = Configuration["AWS:AccessKeyId"],
                AwsSecretAccessKey = Configuration["AWS:SecretAccessKey"],
                RegionEndpoint = Amazon.RegionEndpoint.USWest2 // TODO: Include this configuration in AppSettings
            };

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IPortfolioEventService, PortfolioEventService>();

            services.AddScoped<IPortfolioRepository, PortfolioRepository>(provider => new PortfolioRepository(repositoryConfig));
            services.AddScoped<IPortfolioEventRepository, PortfolioEventRepository>(provider => new PortfolioEventRepository(repositoryConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
