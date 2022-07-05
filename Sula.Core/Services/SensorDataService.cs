using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sula.Core.Extensions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Response;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SensorDataService> _logger;

        public SensorDataService(DatabaseContext databaseContext, IMemoryCache cache, ILogger<SensorDataService> logger)
        {
            _databaseContext = databaseContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<SensorPureDataResponse[]> GetSensorsAndPureDataAsync(ApplicationUser user)
        {
            var cacheKey = $"getsensorsandpuredataasync.{user.Id}";

            var data = _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    return await _databaseContext.SensorData
                        .Where(sensorData => user.Sensors.Select(sensor => sensor.Id).Contains(sensorData.DeviceId))
                        .ToListAsync();
                }
            );

            var temperatureTasks = user.Sensors
                .Select(async sensor => GetSensorAndPureData(sensor, await data, DataType.Temperature));

            var humidityTasks = user.Sensors
                .Select(async sensor => GetSensorAndPureData(sensor, await data, DataType.Humidity));

            return await Task.WhenAll(temperatureTasks.Concat(humidityTasks));
        }

        public async Task<SensorMinMaxDataResponse[]> GetSensorsAndMinMaxDataAsync(ApplicationUser user)
        {
            var cacheKey = $"getsensorsandminmaxdataasync.{user.Id}";

            var data = _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    return await _databaseContext.SensorData
                        .Where(sensorData => user.Sensors.Select(sensor => sensor.Id).Contains(sensorData.DeviceId))
                        .ToListAsync();
                }
            );

            var temperatureTasks = user.Sensors
                .Select(async sensor => GetSensorAndMinMaxData(sensor, await data, DataType.Temperature));

            var humidityTasks = user.Sensors
                .Select(async sensor => GetSensorAndMinMaxData(sensor, await data, DataType.Humidity));

            return await Task.WhenAll(temperatureTasks.Concat(humidityTasks));
        }

        public async Task<IEnumerable<SensorPureDataResponse>> GetSensorAndPureDataAsync(ApplicationUser user, string id)
        {
            var cacheKey = $"getsensorandpuredataasync.{user.Id}.{id}";

            var data = _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    return await _databaseContext.SensorData
                        .Where(sensorData => sensorData.DeviceId == id)
                        .ToListAsync();
                }
            );

            var sensor = user.Sensors.SingleOrDefault(sensor => sensor.Id == id);

            return new List<SensorPureDataResponse>() {
                GetSensorAndPureData(sensor, await data, DataType.Temperature), 
                GetSensorAndPureData(sensor, await data, DataType.Humidity)
            };
        }


        private SensorPureDataResponse GetSensorAndPureData(Sensor sensor, IEnumerable<SensorData> sensorData,
            DataType dataType)
        {
            var cacheKey = $"getsensorandpuredata.{sensor.Id}.{dataType}";

            var cacheEntry = _cache.GetOrCreate(cacheKey, entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    return new SensorPureDataResponse(GetSensorDataByType(sensorData, sensor.Id, dataType));
                }
            );

            cacheEntry.Sensor = sensor;
            cacheEntry.DataType = dataType;

            return cacheEntry;
        }

        private SensorMinMaxDataResponse GetSensorAndMinMaxData(Sensor sensor, IEnumerable<SensorData> sensorData,
            DataType dataType)
        {
            var cacheKey = $"getsensorandminmaxdata.{sensor.Id}.{dataType}";

            var cacheEntry = _cache.GetOrCreate(cacheKey, entry =>
                {
                    entry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    return new SensorMinMaxDataResponse(GetSensorMinMaxDataByType(sensorData, sensor.Id, dataType));
                }
            );

            cacheEntry.Sensor = sensor;
            cacheEntry.DataType = dataType;

            return cacheEntry;
        }

        private IEnumerable<SensorData> GetSensorDataByType(IEnumerable<SensorData> allSensorData, string sensorId, DataType dataType)
        {
            return allSensorData
                .Where(data => data.DeviceId == sensorId)
                .Where(data => data.Type == dataType)
                .Where(data => data.Time > DateTimeOffset.Now.AddDays(-180))
                .GroupBy(data => data.Time.ToHour())
                .Select(group => new SensorData
                {
                    DeviceId = sensorId,
                    Time = group.Key,
                    Type = dataType,
                    Value = group.Average(data => data.Value)
                })
                .OrderBy(data => data.Time);
        }


        private Data GetSensorMinMaxDataByType(IEnumerable<SensorData> allSensorData, string sensorId, DataType dataType)
        {       
            var week = allSensorData
                .Where(data => data.DeviceId == sensorId)
                .Where(data => data.Type == dataType)
                .Where(data => data.Time > DateTimeOffset.Now.AddDays(-6))
                .GroupBy(data => data.Time.Date.ToShortDateString())
                .Select(group => new SensorMinMaxData
                {
                    DeviceId = sensorId,
                    Time = DateTimeOffset.Parse(group.Key),
                    Type = dataType,
                    Min = group.Min(data => data.Value),
                    Max = group.Max(data => data.Value),
                    Mean = group.Average(data => data.Value),
                })
                .OrderBy(data => data.Time);

            var month = allSensorData
                .Where(data => data.DeviceId == sensorId)
                .Where(data => data.Type == dataType)
                .Where(data => data.Time > DateTimeOffset.Now.AddMonths(-1))
                .GroupBy(data => data.Time.Date.ToShortDateString())
                .Select(group => new SensorMinMaxData
                {
                    DeviceId = sensorId,
                    Time = DateTimeOffset.Parse(group.Key),
                    Type = dataType,
                    Min = group.Min(data => data.Value),
                    Max = group.Max(data => data.Value),
                    Mean = group.Average(data => data.Value),
                })
                .OrderBy(data => data.Time);

            var year = allSensorData
                .Where(data => data.DeviceId == sensorId)
                .Where(data => data.Type == dataType)
                .Where(data => data.Time > DateTimeOffset.Now.AddYears(-1))
                .GroupBy(data => data.Time.ToMonth())
                .Select(group => new SensorMinMaxData
                    {
                        DeviceId = sensorId,
                        Time = group.Key,
                        Type = dataType,
                        Min = group.Min(data => data.Value),
                        Max = group.Max(data => data.Value),
                        Mean = group.Average(data => data.Value),
                    })
                .OrderBy(data => data.Time);

            return new Data
            {
                Week = week,
                Month = month,
                Year = year
            };
;
        }
    }
}