using System.Collections.Generic;
using System.Linq;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class AlertService : IAlertService
    {
        public IEnumerable<Alert> GetAllAlertsForUser(ApplicationUser user)
        {
            return user.Sensors
                .SelectMany(sensor => sensor.Limits
                    .Where(limit => limit.AlertTime != null)
                    .Select(limit => new Alert(limit, sensor)));
        }
    }
}
