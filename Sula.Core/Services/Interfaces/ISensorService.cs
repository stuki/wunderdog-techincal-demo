using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;

namespace Sula.Core.Services.Interfaces
{
    public interface ISensorService
    {
        Task<Sensor> AddSensorAsync(ApplicationUser user, SensorAddRequest request);
        Task<Sensor> AddSensorAsync(string sensorId);
        Task<Sensor> EditSensorAsync(ApplicationUser user, string sensorId, SensorUpdateRequest request);
    }
}