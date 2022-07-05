using System;
using Sula.Core.Models;
using Sula.Core.Models.Support;

namespace Sula.Core.Extensions
{
    public static class HubEventExtensions
    {
        public static bool IsEmpty(this HubEvent hubEvent)
        {
            if (hubEvent == null)
            {
                return true;
            }
            
            return string.IsNullOrWhiteSpace(hubEvent.Data) 
                   || string.IsNullOrWhiteSpace(hubEvent.DeviceId) 
                   || hubEvent.Time == DateTimeOffset.MinValue;
        }
    }
}