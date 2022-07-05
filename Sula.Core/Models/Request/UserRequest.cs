using System.ComponentModel.DataAnnotations;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Request
{
    public class UserRequest
    {
        [Required]
        public string Email { get; set; } 
        
        [Required]
        public TemperatureUnit TemperatureUnit { get; set; } 
    }
}