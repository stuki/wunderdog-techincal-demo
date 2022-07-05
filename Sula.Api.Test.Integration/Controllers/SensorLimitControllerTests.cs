using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Exceptions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Test.Utils;
using Newtonsoft.Json;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class SensorLimitControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseContext _databaseContext;

        public SensorLimitControllerTests(IntegrationTestBase<Startup> factory)
        {
            _client = factory.CreateClient();

            _databaseContext = factory.CreateDatabaseContext();
        }
        
        [Fact]
        public async Task Edit_Returns_200AndLimit_WhenSuccessful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            var sensor = await _databaseContext.Sensors
                .Include(device => device.Limits)
                .AsNoTracking()
                .SingleOrDefaultAsync(device => device.Id == "id");
            
            var limit = sensor.Limits.First();

            var request = new SensorLimitUpdateRequest
            {
                Value = 15,
                Operator = Operator.MoreThan,
                DataType = DataType.Temperature,
                IsEnabled = false
            };
            
            var response = await _client.PutAsJsonAndAssertSuccessAsync("/api/v1/sensor/id/limit/" + limit.Id, request);
            
            var newLimit = await response.GetContentAsJsonAsync<Limit>();

            Assert.Equal(newLimit.DataType, request.DataType);
            Assert.Equal(newLimit.Operator, request.Operator);
            Assert.Equal(newLimit.Value, request.Value);
            Assert.Equal(newLimit.DataType, request.DataType);
        }

        [Fact]
        public async Task Add_Returns_200AndLimit_WhenSuccessful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            var request = new SensorLimitAddRequest
            {
                DataType = DataType.Temperature,
                Operator = Operator.Equal,
                Value = 12,
                IsEnabled = true
            };
            
            var response = await _client.PostAsJsonAndAssertSuccessAsync("/api/v1/sensor/id/limit", request);
            var limit = await response.GetContentAsJsonAsync<Limit>();

            Assert.Equal(limit.DataType, request.DataType);
            Assert.Equal(limit.Operator, request.Operator);
            Assert.Equal(limit.Value, request.Value);
            Assert.Equal(limit.DataType, request.DataType);
        }

        [Fact]
        public async Task Edit_Returns400WithErrorCode_WhenSensorDoesNotExist()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            var request = new SensorLimitUpdateRequest
            {
                DataType = DataType.Temperature,
                Operator = Operator.Equal,
                Value = 12,
                IsEnabled = false
            };
            
            var response = await _client.PutAsJsonAsync("/api/v1/sensor/humbug/limit/1", request);
            var content = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new {Code = ""});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorCode.SensorDoNotExist, content.Code);
        }

    }
}