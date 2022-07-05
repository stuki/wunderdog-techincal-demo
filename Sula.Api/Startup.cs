using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sula.Core.Extensions;
using Sula.Core.Models.Entity;
using Sula.Core.Platform;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;
using OpenIddict.Validation.AspNetCore;

[assembly: ApiController]

namespace Sula.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCustomOptions(Configuration);

            services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Default"));
                    options.UseOpenIddict();
                }
            );

            services.AddHealthChecks();

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson();

            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen();
            }

            services.AddApplicationInsightsTelemetry();

            services.ConfigureIdentity(Environment);

            services.AddMemoryCache();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITextMessageService, TextMessageService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddScoped<ISensorDataService, SensorDataService>();
            services.AddScoped<ISensorLimitService, SensorLimitService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
        }


        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsProduction())
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sula V1"); });
            }

            app.UseRouting();

            app.UseCors(o => 
                {
                    o.AllowAnyOrigin();
                    o.AllowAnyMethod();
                    o.AllowAnyHeader();
                }
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}