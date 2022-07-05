using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Sula.Api;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Response;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Test.Utils;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class SensorDataControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseContext _databaseContext;

        public SensorDataControllerTests(IntegrationTestBase<Startup> factory)
        {
            _client = factory.CreateClient();

            _databaseContext = factory.CreateDatabaseContext();
        }

        [Fact]
        public async Task GetSensorsAndData_Returns_200WithAllSensorData_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            const string firstSensorId = "id";
            const string secondSensorId = "ego";
            var firstSensorTemperatureData = await AddRandomSensorData(5, firstSensorId);
            var firstSensorHumidityData = await AddRandomSensorData(2, firstSensorId, DataType.Humidity);
            var secondSensorTemperatureData = await AddRandomSensorData(3, secondSensorId);
            var secondSensorHumidityData = await AddRandomSensorData(8, secondSensorId, DataType.Humidity);

            var response = await _client.GetAndAssertSuccessAsync("/api/v1/sensor/data");

            var data = await response.GetContentAsJsonAsync<SensorPureDataResponse[]>();

            Assert.NotEmpty(data);

            var first = data.Where(cluster => cluster.Sensor.Id == firstSensorId);

            Assert.NotNull(first);

            var second = data.Where(cluster => cluster.Sensor.Id == secondSensorId);

            Assert.NotNull(second);
        }

        [Fact]
        public async Task GetSensorsAndData_Returns_CachedVersionsWhenQuicklyPolled()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            const string sensorId = "id";
            await AddRandomSensorData(1000, sensorId);

            var response = await _client.GetAndAssertSuccessAsync($"/api/v1/sensor/data");

            var data = await response.GetContentAsJsonAsync<SensorPureDataResponse[]>();

            Assert.NotEmpty(data);

            await AddRandomSensorData(1000, sensorId);

            var secondData = await response.GetContentAsJsonAsync<SensorPureDataResponse[]>();
            Assert.NotEmpty(secondData);
            Assert.Equal(data.Length, secondData.Length);
        }

        [Fact]
        public async Task GetSensorsAndData_Returns_AllUserSensors_When_NoSensorsProvidedInQuery()
        {
            await _client.GetToken(TestUsers.Other.Email, TestUsers.Password);
            
            const string firstSensorId = "ed";
            const string secondSensorId = "ned";
            var firstSensorTemperatureData = await AddRandomSensorData(5, firstSensorId);
            var firstSensorHumidityData = await AddRandomSensorData(3, firstSensorId, DataType.Humidity);
            var secondSensorTemperatureData = await AddRandomSensorData(2, secondSensorId);
            var secondSensorHumidityData = await AddRandomSensorData(6, secondSensorId, DataType.Humidity);

            var response = await _client.GetAndAssertSuccessAsync("/api/v1/sensor/data");

            var data = await response.GetContentAsJsonAsync<SensorPureDataResponse[]>();

            Assert.NotEmpty(data);

            var first = data.Where(cluster => cluster.Sensor.Id == firstSensorId);

            Assert.NotNull(first);

            var second = data.Where(cluster => cluster.Sensor.Id == secondSensorId);

            Assert.NotNull(second);
        }

        private async Task<SensorData[]> AddRandomSensorData(int count, string sensorId, DataType dataType = DataType.Temperature)
        {
            var sensorData = new List<SensorData>();

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                sensorData.Add(
                    new SensorData
                    {
                        Time = DateTimeOffset.UtcNow.AddMinutes(30 * i),
                        Value = random.Next(30),
                        DeviceId = sensorId,
                        Type = dataType
                    });
            }

            await _databaseContext.SensorData.AddRangeAsync(sensorData);
            await _databaseContext.SaveChangesAsync();

            return sensorData.ToArray();
        }
    }
}