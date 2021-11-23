using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleLock.Extensions;
using SimpleLock.Processors;
using SimpleLock.Services;

namespace SimpleLock
{
    internal class Startup
    {
        IConfiguration Configuration;

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddSwaggerGen();

             /// Adds a processoor for "Resource" objects and decorates it with RedLock.
            /// With this we make sure that whenever an operation is trying to be executed against
            /// one "Resource" in our redis database, we can lock the resource.
            services.AddScoped<IResourceProcessor, DatabaseResourceProcessor>();
            services.Decorate<IResourceProcessor, RedLockResourceProcessor>();

            services.AddTransient<IResourceService<Resource>, SaveResourceService>();

            //services.AddHostedService<ResourceWorker>();

            services.AddRedisCache(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(SimpleLock)));

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => 
            {
                x.MapSwagger();
                x.MapControllers();
            });

        }
    }
}