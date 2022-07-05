using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Sula.Core.Models.Entity;
using Sula.Core.Platform;

namespace Sula.Core.Extensions
{
    public static class IdentityExtension
    {
        public static void ConfigureIdentity(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 10;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
            });

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DatabaseContext>();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("/connect/token");

                    options
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    options.SetAccessTokenLifetime(TimeSpan.FromDays(1));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(365));

                    options.AcceptAnonymousClients();

                    if (environment.IsDevelopment())
                    {
                        try
                        {
                            options.AddDevelopmentEncryptionCertificate()
                                .AddDevelopmentSigningCertificate();
                        }
                        catch (Exception)
                        {
                            options.AddEphemeralEncryptionKey()
                                .AddEphemeralSigningKey();
                        }
                    }
                    else
                    {
                        options.AddEphemeralEncryptionKey()
                            .AddEphemeralSigningKey();
                    }

                    var builder = options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationRequestCaching();

                    if (environment.IsDevelopment())
                    {
                        builder.DisableTransportSecurityRequirement();
                    }
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}