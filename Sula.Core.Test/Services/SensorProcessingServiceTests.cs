using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;
using Sula.Core.Test.Utils;
using Moq;
using Xunit;

namespace Sula.Core.Test.Services
{
    public class SensorServiceLimitTests
    {
        private readonly DatabaseContext _databaseContext;
        private readonly Mock<ITextMessageService> _messageServiceMock;
        private readonly SensorProcessingService _service;
        private readonly SqliteConnection _connection;

        public SensorServiceLimitTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _databaseContext = TestFactory.CreateSqliteDatabaseContext(_connection);
            _databaseContext.Database.EnsureCreated();
            
            _messageServiceMock = new Mock<ITextMessageService>();
            _service = new SensorProcessingService(
                _databaseContext,
                _messageServiceMock.Object
            );
        }

        [Fact]
        public async Task EvaluateLimit_Should_SendMessage_When_ValuesAreOutsideOfLimits()
        {
            var sensorId = "sensorId";
            var sensorData = TestFactory.CreateSensorData(sensorId);
            var sensor = new Sensor(sensorId)
            {
                Limits = new List<Limit>
                {
                    new Limit
                    {
                        DataType = DataType.Temperature,
                        IsEnabled = true,
                        Operator = Operator.LessThan,
                        Value = 20
                    }
                },
                Name = "Test Device",
                Data = new List<SensorData>()
            };
            await _databaseContext.Sensors.AddAsync(sensor);
            await _databaseContext.SaveChangesAsync();

            await _service.EvaluateLimitsAsync(sensorId, new[] {sensorData});
            var expectedTuple = new List<(SensorData data, Limit limit)> {(sensorData, sensor.Limits.First())};

            _messageServiceMock.Verify(mock => mock.SendMessageAsync(It.IsAny<Sensor>(), expectedTuple), Times.Once);
        }

        [Fact]
        public async Task EvaluateLimit_ShouldNot_SendMessage_When_ValuesAreWithinLimits()
        {
            var sensorId = "sensorId";
            var sensorData = TestFactory.CreateSensorData(sensorId);
            var sensor = new Sensor(sensorId)
            {
                Limits = new List<Limit>
                {
                    new Limit
                    {
                        DataType = DataType.Temperature,
                        IsEnabled = true,
                        Operator = Operator.LessThan,
                        Value = 50
                    }
                },
                Name = "Test Device",
                Data = new List<SensorData>()
            };
            await _databaseContext.Sensors.AddAsync(sensor);
            await _databaseContext.SaveChangesAsync();

            await _service.EvaluateLimitsAsync(sensorId, new[] {sensorData});

            _messageServiceMock.Verify(mock =>
                    mock.SendMessageAsync(
                        It.IsAny<Sensor>(),
                        It.IsAny<List<(SensorData data, Limit limit)>>()),
                Times.Never);
        }
    }
}