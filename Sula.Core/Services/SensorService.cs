using System;
using System.Linq;
using System.Threading.Tasks;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class SensorService : ISensorService
    {
        private readonly DatabaseContext _databaseContext;

        public SensorService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Sensor> AddSensorAsync(ApplicationUser user, SensorAddRequest request)
        {
            Sensor sensor;

            if (_databaseContext.Sensors.All(device => device.Id != request.SensorId))
            {
                sensor = new Sensor(request.SensorId);
                user.Sensors.Add(sensor);
            }
            else
            {
                sensor = await _databaseContext.Sensors.FindAsync(request.SensorId);

                if (sensor.UserId != null)
                {
                    throw new SensorAlreadyExistsException();
                }

                user.Sensors.Add(sensor);
            }

            await _databaseContext.SaveChangesAsync();

            return sensor;
        }

        public async Task<Sensor> AddSensorAsync(string sensorId)
        {
            var sensor = new Sensor(sensorId);
            await _databaseContext.AddAsync(sensor);
            return sensor;
        }

        public async Task<Sensor> EditSensorAsync(ApplicationUser user, string sensorId, SensorUpdateRequest request)
        {
            if (user.Sensors.All(device => device.Id != sensorId))
            {
                throw new SensorDoNotExistException();
            }

            var sensor = await _databaseContext.GetSensor(sensorId);
            sensor.Update(request);
            
            await _databaseContext.SaveChangesAsync();
            
            return sensor;
        }
    }
}