using System;
using System.Net;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Remato
{
    public class Startup
    {
        private readonly RematoConfig _config;

        public Startup(IConfiguration configuration)
        {
            var test = configuration["REMATO_ENDPOINT"];
            var endpoint = IPEndPoint.Parse(test);
            
            _config = new RematoConfig()
            {
                TokenOptions = new TokenOptions()
                {
                    Audience = "",
                    Expiration = TimeSpan.FromDays(2),
                    Issuer = "localhost",
                    Secret = "lockdown-1234"
                }
            }; // TODO
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddRematoCore(_config);
            services.AddRematoSecurity(_config);
            services.AddRematoCache(_config);
            services.AddRematoDatabase(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            migrationRunner.MigrateUp();
        }
    }
}