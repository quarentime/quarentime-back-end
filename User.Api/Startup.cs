using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using User.Api.Filters;
using User.Api.Services;
using Microsoft.OpenApi.Models;
using Quarentime.Common.Repository;
using Quarentime.Common.Services;
using System.Threading.Tasks;
using User.Api.Logging;

namespace User.Api
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
            services.AddControllers(options =>
                    {
                        options.Filters.Add(typeof(ExceptionFilter));
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy(),
                        };
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(new SnakeCaseNamingStrategy()));
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    })
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                        options.Authority = Environment.GetEnvironmentVariable("JWT_AUTHORITY");
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                // This is the forwarded value from cloud endpoints
                                string forwardedAuthorization = context.Request.Headers["X-Forwarded-Authorization"];
                                if (!string.IsNullOrWhiteSpace(forwardedAuthorization))
                                {
                                    if (forwardedAuthorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                    {
                                        context.Token = forwardedAuthorization.Substring("Bearer ".Length).Trim();
                                    }
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddSwaggerGen(c =>
                    {
                        c.EnableAnnotations();
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users microservice", Version = "v1" });

                    })
                    .AddSwaggerGenNewtonsoftSupport();

            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services
                .AddScoped(typeof(ICollectionRepository<>), typeof(CollectionRepository<>))
                .AddScoped(typeof(ISubCollectionRepository<>), typeof(SubCollectionRepository<>))
                .AddScoped<IUserService, UserService>()
                .AddTransient<ICloudTaskService, CloudTaskService>()
                .AddScoped<IDevicesService, DevicesService>()
                .AddScoped<IPhoneVerificationService, PhoneVerificationService>()
                .AddScoped<IConfigurationService, ConfigurationService>()
                .AddScoped<IContactsService, ContactsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AnyOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users microservice");
                c.SupportedSubmitMethods();
            });


        }
    }
}
