using System.ComponentModel.DataAnnotations;
using Sula.Core.Models.Entity;

namespace Sula.Core.Models.Request
{
    public class SensorAddRequest
    {
        [Required]
        public string SensorId { get; set; }
        public Limit Limit { get; set; }
    }
}