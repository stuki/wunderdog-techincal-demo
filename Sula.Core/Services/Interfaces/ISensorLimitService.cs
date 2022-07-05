using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;

namespace Sula.Core.Services.Interfaces
{
    public interface ISensorLimitService
    {
        Task<Limit> AddLimitAsync(ApplicationUser user, string sensorId, SensorLimitAddRequest request);
        Task<Limit> EditLimitAsync(ApplicationUser user, string sensorId, int limitId, SensorLimitUpdateRequest request);
        Task<bool> DeleteLimitAsync(ApplicationUser user, string sensorId, int limitId);
    }
}