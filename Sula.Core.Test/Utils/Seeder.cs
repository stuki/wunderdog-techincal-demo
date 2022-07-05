using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Platform;

namespace Sula.Core.Test.Utils
{
    public static class Seeder
    {
        public static async Task InitializeDatabaseForTests(DatabaseContext databaseContext, UserManager<ApplicationUser> userManager)
        {
            CleanDatabase(databaseContext);
            await TestUsers.CreateTestUsers(userManager);
            await databaseContext.SaveChangesAsync();
        }

        private static void CleanDatabase(DatabaseContext databaseContext)
        {
            databaseContext.Users.RemoveRange(databaseContext.Users.Where(user => user.Email != TestUsers.Default.Email));
            databaseContext.Sensors.RemoveRange(databaseContext.Sensors.Where(device => !TestUsers.Default.Sensors.Contains(device)));
            databaseContext.SensorData.RemoveRange(databaseContext.SensorData);
        }
    }
}