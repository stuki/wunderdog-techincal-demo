using System.Collections.Generic;
using Sula.Core.Models;
using Sula.Core.Models.Entity;

namespace Sula.Core.Services.Interfaces
{
    public interface IAlertService
    {
        IEnumerable<Alert> GetAllAlertsForUser(ApplicationUser user);
    }
}