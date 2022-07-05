using System.Collections.Generic;
using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Response;

namespace Sula.Core.Services.Interfaces
{
    public interface ISensorDataService
    {
        Task<SensorPureDataResponse[]> GetSensorsAndPureDataAsync(ApplicationUser user);
        Task<SensorMinMaxDataResponse[]> GetSensorsAndMinMaxDataAsync(ApplicationUser user);
        Task<IEnumerable<SensorPureDataResponse>> GetSensorAndPureDataAsync(ApplicationUser applicationUser, string id);
    }
}