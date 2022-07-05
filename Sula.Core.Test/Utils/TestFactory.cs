using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Moq;

namespace Sula.Core.Test.Utils
{
    public static class TestFactory
    {
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            return type == LoggerTypes.List ? new ListLogger() : NullLoggerFactory.Instance.CreateLogger("Null Logger");
        }

        public static DatabaseContext CreateInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new DatabaseContext(options);
        }

        // http://terencegolla.com/blog/unit-testing-asp-net-core-identity
        public static void SetupDatabaseAndUserManager(
            DbConnection connection, 
            out DatabaseContext databaseContext,
            out UserManager<ApplicationUser> userManager)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection
                .AddDbContext<DatabaseContext>(options => options.UseSqlite(connection));
            serviceCollection.AddLogging();

            serviceCollection.ConfigureIdentity(new Mock<IWebHostEnvironment>().Object);

            databaseContext = serviceCollection.BuildServiceProvider().GetService<DatabaseContext>();
            databaseContext.Database.EnsureCreated();
            userManager = serviceCollection.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
        }

        public static DatabaseContext CreateSqliteDatabaseContext(DbConnection connection)
        {
            return new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(connection)
                .Options);
        }

        public static HubEvent CreateHubEvent(string sensorId, string data = null, DateTimeOffset? time = null)
        {
            return new HubEvent
            {
                DeviceId = sensorId,
                Data = data ?? "14020932",
                Time = time ?? DateTimeOffset.Now
            };
        }

        public static SensorData CreateSensorData(string sensorId, DateTimeOffset? time = null)
        {
            return new SensorData
            {
                DeviceId = sensorId,
                Type = DataType.Temperature,
                Value = (decimal) 22.5,
                Time = time ?? DateTimeOffset.Now
            };
        }

        public static Sensor CreateSensorWithData(string sensorId)
        {
            var sensor = new Sensor(sensorId)
            {
                Limits = new List<Limit>
                {
                    new Limit
                    {
                        DataType = DataType.Temperature,
                        IsEnabled = true,
                        Operator = Operator.Equal,
                        Value = 10
                    }
                },
                Name = "Test Device",
                Data = new List<SensorData>()
            };

            for (var i = 0; i < 100; i++)
            {
                var sensorData = new SensorData
                {
                    DeviceId = sensorId,
                    Time = DateTimeOffset.Now.AddMinutes(i),
                    Type = i % 2 == 0 ? DataType.Humidity : DataType.Temperature,
                    Value = (decimal) Math.Sin(i)
                };

                sensor.Data.Add(sensorData);
            }

            return sensor;
        }
    }
}