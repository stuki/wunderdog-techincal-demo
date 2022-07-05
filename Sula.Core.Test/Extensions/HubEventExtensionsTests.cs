using System;
using System.Threading.Tasks;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Support;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class HubEventExtensionsTests
    {
        [Fact]
        public void IsEmpty_ReturnsTrue_When_HubEvent_Is_Null()
        {
            HubEvent hubEvent = null;
            Assert.True(hubEvent.IsEmpty());
        }
        
        [Fact]
        public void IsEmpty_ReturnsTrue_When_HubEventDataProperty_Is_NotSet()
        {
            HubEvent hubEvent = new HubEvent()
            {
                Data = "",
                DeviceId = "device",
                SequenceNumber = 1,
                Time = DateTimeOffset.Now
            };
            
            Assert.True(hubEvent.IsEmpty());
        }
        
        [Fact]
        public void IsEmpty_ReturnsTrue_When_HubEventDeviceIdProperty_Is_NotSet()
        {
            HubEvent hubEvent = new HubEvent()
            {
                Data = "12AE143",
                DeviceId = "",
                SequenceNumber = 1,
                Time = DateTimeOffset.Now
            };
            
            Assert.True(hubEvent.IsEmpty());
        }
        
        [Fact]
        public void IsEmpty_ReturnsTrue_When_HubEventTimeProperty_Is_NotSet()
        {
            HubEvent hubEvent = new HubEvent()
            {
                Data = "12AE143",
                DeviceId = "",
                SequenceNumber = 1,
                Time = new DateTimeOffset()
            };
            
            Assert.True(hubEvent.IsEmpty());
        }
        
        [Fact]
        public void IsEmpty_ReturnsFalse_When_HubEvent_Is_Populated()
        {
            HubEvent hubEvent = new HubEvent()
            {
                Data = "12AE143",
                DeviceId = "device",
                SequenceNumber = 1,
                Time = DateTimeOffset.Now
            };
            
            Assert.False(hubEvent.IsEmpty());
        }
    }
}