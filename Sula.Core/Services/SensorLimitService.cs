using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class SensorLimitService : ISensorLimitService
    {
        private readonly DatabaseContext _databaseContext;

        public SensorLimitService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Limit> AddLimitAsync(ApplicationUser user, string sensorId, SensorLimitAddRequest request)
        {
            if (!user.PhoneNumberConfirmed)
            {
                throw new NoPhoneNumberException();
            }
            
            var sensor = user.Sensors.FirstOrDefault(device => device.Id == sensorId);
            
            if (sensor == null)
            {
                throw new SensorDoNotExistException();
            }
            
            var limit = sensor.Limits?.FirstOrDefault(storedLimit =>
                storedLimit.Value == request.Value
                && storedLimit.Operator == request.Operator
                && storedLimit.DataType == request.DataType);

            if (limit != null)
            {
                throw new LimitAlreadyExistException();
            }

            var newLimit = new Limit(request);

            if (sensor.Limits != null)
            {
                sensor.Limits.Add(newLimit);
            }
            else
            {
                sensor.Limits = new List<Limit> { newLimit };
            }

            await _databaseContext.SaveChangesAsync();

            return newLimit;
        }

        public async Task<Limit> EditLimitAsync(ApplicationUser user, string sensorId, int limitId, SensorLimitUpdateRequest request)
        {
            var limit = GetLimit(user, sensorId, limitId, out _);

            limit.Update(request);

            await _databaseContext.SaveChangesAsync();

            return limit;
        }

        public async Task<bool> DeleteLimitAsync(ApplicationUser user, string sensorId, int limitId)
        {
            var limit = GetLimit(user, sensorId, limitId, out var sensor);

            sensor.Limits.Remove(limit);

            await _databaseContext.SaveChangesAsync();

            return true;
        }

        private Limit GetLimit(ApplicationUser user, string sensorId, int limitId, out Sensor sensor)
        {
            sensor = user.Sensors.FirstOrDefault(device => device.Id == sensorId);
            
            if (sensor == null)
            {
                throw new SensorDoNotExistException();
            }

            _databaseContext.Entry(sensor).Collection(s => s.Limits).Load();

            var limit = sensor.Limits.SingleOrDefault(storedLimit => storedLimit.Id == limitId);
            
            if (limit == null)
            {
                throw new LimitDoNotExistException();
            }

            return limit;
        }
    }
}