using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sula.AzureFunctions.Functions;
using Sula.Core.Test.Utils;
using Moq;
using Newtonsoft.Json;
using Sula.Core.Exceptions;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;
using Xunit;

namespace Sula.AzureFunctions.Test.Functions
{
    public class DataProcessingFunctionTests
    {
        private readonly DataProcessingFunction _function;
        private readonly DatabaseContext _databaseContext;
        private readonly SqliteConnection _connection;

        public DataProcessingFunctionTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _databaseContext = TestFactory.CreateSqliteDatabaseContext(_connection);
            _databaseContext.Database.EnsureCreated();

            var messageServiceMock = new Mock<ITextMessageService>();
            var sensorService = new SensorProcessingService(
                _databaseContext,
                messageServiceMock.Object
            );

            _function = new DataProcessingFunction(sensorService);
        }

        [Fact]
        public async Task Should_ThrowDeserializationException_When_EventDataBody_Is_Empty()
        {
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);

            var bytes = new byte[0];
            var eventData = new[]
            {
                new EventData(bytes)
            };

            await Assert.ThrowsAsync<DeserializeException>(async () => await _function.Run(eventData, logger));

            Assert.Contains(logger.Logs, log => log.Contains("An error occured"));
        }

        [Fact]
        public async Task Should_ThrowDbUpdateException_When_SavingToDatabase_Fails()
        {
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);

            var sensorServiceMock = new Mock<ISensorProcessingService>();

            sensorServiceMock
                .Setup(mock => mock.ProcessSensorDataAsync(It.IsAny<HubEvent>()))
                .ThrowsAsync(new DbUpdateException());

            var function = new DataProcessingFunction(sensorServiceMock.Object);

            var bytes = GetEventMessageAsByteArray(out _);
            var eventData = new[]
            {
                new EventData(bytes)
            };

            await Assert.ThrowsAsync<DbUpdateException>(async () => await function.Run(eventData, logger));

            Assert.Contains(logger.Logs, log => log.Contains("An error occured"));
        }

        [Fact]
        public async Task Should_LogWhenStartingAndFinishing()
        {
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);

            var bytes = GetEventMessageAsByteArray(out _);
            var eventData = new[]
            {
                new EventData(bytes)
            };

            await _function.Run(eventData, logger);

            Assert.Contains(logger.Logs, log => log.Contains("Processing"));
            Assert.Contains(logger.Logs, log => log.Contains("processed"));
        }

        [Theory]
        [InlineData("{ \"deviceId\" : \"3358BD\", \"data\" : \"14020932\", \"time\": \"1588789180\", \"deviceType\": \"AirWits\", \"sequenceNumber\":5 }")]
        public async Task Should_Process_RealLifeData_Successfully(string message)
        {
            var data = new[]
            {
                new EventData(Encoding.UTF8.GetBytes(message))
            };

            await _function.Run(data, TestFactory.CreateLogger());

            Assert.True(_databaseContext.SensorData.Any());
            Assert.Equal(2, _databaseContext.SensorData.Count());

            var sensorData = await _databaseContext.SensorData.ToListAsync();

            Assert.Equal("3358BD", sensorData.First().DeviceId);
            Assert.Equal((decimal) 12.1, sensorData.First().Value);
            Assert.Equal(DataType.Temperature, sensorData.First().Type);

            Assert.Equal("3358BD", sensorData.Last().DeviceId);
            Assert.Equal(50, sensorData.Last().Value);
            Assert.Equal(DataType.Humidity, sensorData.Last().Type);
        }
    
        private byte[] GetEventMessageAsByteArray(out string sensorId)
        {
            sensorId = Guid.NewGuid().ToString();
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(TestFactory.CreateHubEvent(sensorId)));
        }
    }
}


