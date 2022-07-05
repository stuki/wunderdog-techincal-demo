using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Exceptions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Response;
using Sula.Core.Platform;
using Sula.Core.Test.Utils;
using Newtonsoft.Json;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class SensorControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private IntegrationTestBase<Startup> _factory;

        public SensorControllerTests(IntegrationTestBase<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddSensor_Returns_200_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            var value = new SensorAddRequest
            {
                SensorId = Guid.NewGuid().ToString()
            };

            await _client.PostAsJsonAndAssertSuccessAsync("/api/v1/sensor", value);
        }

        [Fact]
        public async Task AddSensor_SavesNewSensor_To_User()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            var deviceId = Guid.NewGuid().ToString();

            var value = new SensorAddRequest
            {
                SensorId = deviceId
            };

            await _client.PostAsJsonAndAssertSuccessAsync("/api/v1/sensor", value);
            var response = await _client.GetAndAssertSuccessAsync("/api/v1/account");
            
            var user = await response.GetContentAsJsonAsync<UserResponse>();

            Assert.Contains(deviceId, user.Sensors.Select(sensor => sensor.Id));
        }

        [Fact]
        public async Task AddSensor_Returns_400WithErrorCode_When_SensorAlreadyExists()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            var deviceId = Guid.NewGuid().ToString();
            
            var databaseContext = _factory.CreateDatabaseContext();

            var user = await databaseContext.Users.SingleAsync(applicationUser => applicationUser.Email == TestUsers.Other.Email);
            var sensor = new Sensor(deviceId) {UserId = user.Id};

            await databaseContext.Sensors.AddAsync(sensor);
            await databaseContext.SaveChangesAsync();

            var value = new SensorAddRequest
            {
                SensorId = deviceId
            };

            var response = await _client.PostAsJsonAsync("/api/v1/sensor", value);
            var content = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new {Code = ""});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorCode.SensorAlreadyExists, content.Code);
        }
    }
}