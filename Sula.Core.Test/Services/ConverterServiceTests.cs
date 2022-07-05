using System;
using System.Linq;
using Sula.Core.Models;
using Sula.Core.Models.Support;
using Sula.Core.Services;
using Xunit;

namespace Sula.Core.Test.Services
{
    public class SensorServiceDataProcessingTests
    {
        [Fact]
        public void Should_ProcessData()
        {
            const string id = "sensorId";

            var time = DateTimeOffset.Now;

            var data = new HubEvent
            {
                DeviceId = id,
                Data = "1402712C",
                Time = time,
                SequenceNumber = 1
            };

            var result = ConverterService.ConvertHubEvent(data);

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Length);
            Assert.NotNull(result.SingleOrDefault(sensorData => sensorData.Type == DataType.Temperature));
            Assert.NotNull(result.SingleOrDefault(sensorData => sensorData.Type == DataType.Humidity));
        }

        [Theory]
        [InlineData("1402712C", 22.5)]
        [InlineData("14019A2C", 1)]
        [InlineData("1400DC2C", -18)]
        [InlineData("1401F42C", 10)]
        [InlineData("1400002C", -40)]
        [InlineData("1403202C", 40)]
        public void Should_ConvertHexToDecimalCorrectly_For_Temperature(string data, decimal temperature)
        {
            var hubEvent = new HubEvent
            {
                DeviceId = "sensorId",
                Data = data,
                Time = DateTimeOffset.Now,
                SequenceNumber = 1
            };

            var result = ConverterService.ConvertHubEvent(hubEvent);

            Assert.Equal(temperature, result.First(sensorData => sensorData.Type == DataType.Temperature).Value);
        }

        [Theory]
        [InlineData("1402712C", 44)]
        [InlineData("14019A00", 0)]
        [InlineData("1400DC64", 100)]
        [InlineData("1401F43C", 60)]
        [InlineData("14000017", 23)]
        [InlineData("14032025", 37)]
        public void Should_ConvertHexToDecimalCorrectly_For_Humidity(string data, decimal humidity)
        {
            var hubEvent = new HubEvent
            {
                DeviceId = "sensorId",
                Data = data,
                Time = DateTimeOffset.Now,
                SequenceNumber = 1
            };

            var result = ConverterService.ConvertHubEvent(hubEvent);

            Assert.Equal(humidity, result.First(sensorData => sensorData.Type == DataType.Humidity).Value);
        }
    }
}