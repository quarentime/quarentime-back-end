using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notification.Api.Services;
using Quarentime.Common.Repository;
using Quarentime.Common.Services;

namespace Notification.Api
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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new DefaultContractResolver
                     {
                         NamingStrategy = new SnakeCaseNamingStrategy(),
                     };
                     options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(new SnakeCaseNamingStrategy()));
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 });

            services
                .AddScoped(typeof(ICollectionRepository<>), typeof(CollectionRepository<>))
                .AddScoped(typeof(ISubCollectionRepository<>), typeof(SubCollectionRepository<>))
                .AddScoped<IConfigurationService, ConfigurationService>()
                .AddScoped<IDevicesService, DevicesService>()
                .AddScoped<INotificationService, NotificationService>();
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
