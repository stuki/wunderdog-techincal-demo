using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;
using Sula.Core.Test.Utils;
using Moq;
using Xunit;

namespace Sula.Core.Test.Services
{
    public class AccountServiceTests
    {
        private readonly DatabaseContext _databaseContext;

        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly UserService _userService;
        private readonly SqliteConnection _connection;

        public AccountServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            TestFactory.SetupDatabaseAndUserManager(_connection, out _databaseContext, out var userManager);

            _emailServiceMock = new Mock<IEmailService>();
            _userService = new UserService(userManager, _emailServiceMock.Object);
        }

        [Fact(Skip = "NotWorking")]
        public async Task GetUserAsync_Populates_UserObject()
        {
            var service = Setup(out var user);

            var claimsIdentities = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimsIdentities));

            var applicationUser = await service.GetUserAsync(claimsPrincipal);
            
            Assert.NotNull(applicationUser);
            Assert.NotEmpty(applicationUser.Sensors);
            Assert.NotNull(applicationUser.Address);
            Assert.NotNull(applicationUser.Settings);
        }

        [Fact]
        public async Task CreateUserAsync_Adds_User_To_Database()
        {
            var service = Setup(out var user, false);
            const string baseUrl = "http://localhost";

            await service.CreateUserAsync(user, baseUrl);

            await using var databaseContext = TestFactory.CreateSqliteDatabaseContext(_connection);

            var applicationUser = await databaseContext.Users.FirstOrDefaultAsync();

            Assert.NotNull(applicationUser);
            Assert.Equal(user.Email, applicationUser.Email);
        }

        [Fact]
        public async Task CreateUserAsync_Sends_ConfirmationMessage_To_User()
        {
            var service = Setup(out var user, false);
            const string baseUrl = "http://localhost";

            await service.CreateUserAsync(user, baseUrl);

            _emailServiceMock.Verify(
                mock =>
                    mock.SendUserConfirmationMessageAsync(
                        It.Is<string>(value => value == user.Email),
                        It.IsAny<ConfirmationEmailTemplateOptions>()
                    ),
                Times.Once
            );
        }

        [Fact(Skip = "NotWorking")]
        public async Task GetPhoneNumberConfirmationTokenAsync_Nullifies_CurrentPhoneNumber()
        {
            var service = Setup(out var user);

            var request = new PhoneRequest
            {
                PhoneNumber = "+424242424242"
            };

            await service.GetPhoneNumberConfirmationTokenAsync(user, request);

            await using var databaseContext = TestFactory.CreateSqliteDatabaseContext(_connection);

            var applicationUser = await databaseContext.Users.FirstOrDefaultAsync();

            Assert.NotNull(applicationUser);
            Assert.Null(user.PhoneNumber);
            Assert.False(user.PhoneNumberConfirmed);
        }

        [Fact(Skip = "NotWorking")]
        public async Task GetPhoneNumberConfirmationTokenAsync_Creates_ConfirmationToken()
        {
            var service = Setup(out var user);

            var request = new PhoneRequest
            {
                PhoneNumber = "+424242424242"
            };

            var token = await service.GetPhoneNumberConfirmationTokenAsync(user, request);

            Assert.NotNull(token);
            Assert.Equal(6, token.Length);
            Assert.True(int.TryParse(token, out _));
        }

        [Fact(Skip = "NotWorking")]
        public async Task ConfirmPhoneNumberAsync_Adds_PhoneNumber()
        {
            var service = Setup(out var user);

            const string phoneNumber = "+5551234124";

            var token = await service.GetPhoneNumberConfirmationTokenAsync(
                user,
                new PhoneRequest {PhoneNumber = phoneNumber}
            );

            var request = new PhoneConfirmationRequest
            {
                ConfirmationToken = token,
                PhoneNumber = phoneNumber
            };

            await service.ConfirmPhoneNumberAsync(user, request);

            await using var databaseContext = TestFactory.CreateSqliteDatabaseContext(_connection);

            var applicationUser = await databaseContext.Users.FirstOrDefaultAsync();

            Assert.NotNull(applicationUser);
            Assert.Equal(phoneNumber, user.PhoneNumber);
            Assert.True(user.PhoneNumberConfirmed);
        }

        private AccountService Setup(out ApplicationUser user, bool addUser = true)
        {
            user = TestUsers.Default;

            if (addUser)
            {
                _databaseContext.Users.Add(user);
                _databaseContext.SaveChanges();
            }
            
            var service = new AccountService(_userService, _emailServiceMock.Object, _databaseContext);

            return service;
        }
    }
}