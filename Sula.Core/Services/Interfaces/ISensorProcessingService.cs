using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Support;

namespace Sula.Core.Services.Interfaces
{
    public interface ISensorProcessingService
    {
        Task ProcessSensorDataAsync(HubEvent hubEvent);
    }
}