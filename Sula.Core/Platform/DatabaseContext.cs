using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sula.Core.Models;
using Sula.Core.Models.Entity;

namespace Sula.Core.Platform
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SensorData>()
                .HasKey(sensorData => new {sensorData.DeviceId, sensorData.Type, sensorData.Time});

            builder.Entity<SensorData>()
                .Property(data => data.Value)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Limit>()
                .Property<string>("DeviceId");

            builder.Entity<Limit>()
                .Property(limit => limit.Value)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Sensor>()
                .HasMany(sensor => sensor.Data)
                .WithOne()
                .IsRequired()
                .HasForeignKey(data => data.DeviceId);

            builder.Entity<Sensor>()
                .HasMany(sensor => sensor.Limits)
                .WithOne()
                .IsRequired()
                .HasForeignKey("DeviceId");

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Sensors)
                .WithOne()
                .IsRequired()
                .HasForeignKey("UserId");
            
            base.OnModelCreating(builder);

            // Needed for tests
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                    foreach (var property in properties)
                    {
                        builder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }
    }
}