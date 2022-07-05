using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;
using Moq;

namespace Sula.Core.Test.Utils
{
    public class IntegrationTestBase<T> : WebApplicationFactory<T> where T : class
    {
        private readonly SqliteConnection _connection;

        public IntegrationTestBase()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(serviceDescriptor =>
                    serviceDescriptor.ServiceType == typeof(DbContextOptions<DatabaseContext>)
                );

                services.Remove(descriptor);

                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlite(_connection);
                    options.UseOpenIddict();
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var databaseContext = scopedServices.GetRequiredService<DatabaseContext>();
                var userManager = scopedServices.GetService<UserManager<ApplicationUser>>();
                var logger = scopedServices.GetRequiredService<ILogger<IntegrationTestBase<T>>>();

                await databaseContext.Database.EnsureCreatedAsync();

                try
                {
                    await Seeder.InitializeDatabaseForTests(databaseContext, userManager);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred seeding the database. Error: {ex.Message}");
                }
            });

            builder.ConfigureTestServices(services =>
                services.AddScoped(provider => new Mock<ITextMessageService>().Object)
            );
        }

        public DatabaseContext CreateDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            return new DatabaseContext(options);
        }
    }
}