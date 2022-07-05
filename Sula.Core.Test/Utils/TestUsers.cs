using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Test.Utils
{
    public static class TestUsers
    {
        public const string Password = "RealPassword1!";

        public static ApplicationUser[] All => new[] {Default, Other};

        public static ApplicationUser Default =>
            new ApplicationUser
            {
                Email = "valid.user@example.com",
                NormalizedEmail = "VALID.USER@EXAMPLE.COM",
                UserName = "valid.user@example.com",
                NormalizedUserName = "VALID.USER@EXAMPLE.COM",
                Sensors = new List<Sensor>
                {
                    new Sensor("ego"),
                    new Sensor("id")
                    {
                        Limits = new List<Limit>
                        {
                            new Limit
                            {
                                DataType = DataType.Humidity,
                                IsEnabled = true,
                                Operator = Operator.LessThan,
                                Value = 0
                            }
                        }
                    }
                },
                Settings = new Settings(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                PhoneNumber = "+42424242424"
            };

        public static ApplicationUser Other =>
            new ApplicationUser
            {
                Email = "another.valid.user@example.com",
                NormalizedEmail = "ANOTHER.VALID.USER@EXAMPLE.COM",
                UserName = "another.valid.user@example.com",
                NormalizedUserName = "ANOTHER.VALID.USER@EXAMPLE.COM",
                Sensors = new List<Sensor>
                {
                    new Sensor("ed"),
                    new Sensor("ned")
                },
                Settings = new Settings(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                PhoneNumber = "+42424242424"
            };

        public static async Task CreateTestUsers(UserManager<ApplicationUser> userManager)
        {
            foreach (var applicationUser in All)
            {
                await userManager.CreateAsync(applicationUser, Password);
            }
        }
    }
}