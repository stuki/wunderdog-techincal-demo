using Sula.Core.Models.Entity;

namespace Sula.Core.Models.Request
{
    public class UpdateUserRequest
    {
        public Address Address { get; set; }
        public Settings Settings { get; set; }
    }
}