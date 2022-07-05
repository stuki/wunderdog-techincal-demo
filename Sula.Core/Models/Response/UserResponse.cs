using System.Collections.Generic;
using System.Linq;
using Sula.Core.Models.Entity;

namespace Sula.Core.Models.Response
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Settings Settings { get; set; }
        public IEnumerable<Sensor> Sensors { get; set; }

        public UserResponse(ApplicationUser user)
        {
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Settings = user.Settings;
            Sensors = user.Sensors;
        }
        
        public UserResponse()
        {
        }
    }
}