using ItLabs.MultiTenant.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

using ItLabs.MultiTenant.Core.MongoDb;

namespace ItLabs.MultiTenant.Api
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
            services.AddControllers();

            services.TryAddSingleton(provider => Configuration);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Use the HTTP Request Host strategy to identify the tenant
            //Can be switched to a different ITenantIdentificationStrategy implementation (i.e. HTTP header, JWT token claim etc.)
            services.TryAddSingleton<ITenantIdentificationStrategy, RequestHostTenantIdentificationStrategy>();

            //Use the AWS Secrets Manager to store the tenant data
            //Can be switched to a different ITenantStorage implementation (i.e. app.settings, Azure App Service, SQL Database etc.)
            services.TryAddSingleton<ITenantStorage<Tenant>, AWSSecretsManagerTenantStorage>();

            //Use ConcurrentDictionary for caching the tenant data
            //Can be switched to a different ICache implementation (i.e. global cache like Redis, Memcached)
            services.TryAddSingleton<ICache>(cache => new ConcurrentDictionaryCache(new ConcurrentDictionary<string, object>()));

            //Must be transient, to create instance on each request and switch between tenants
            services.TryAddTransient<TenantService<Tenant>>();

            services.TryAddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
            
            services.AddDbContext<TasksDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
