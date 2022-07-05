using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sula.Core.Exceptions;
using Sula.Core.Models;
using Sula.Core.Models.Request;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;

namespace Sula.Api.Controllers
{
    [Route("api/v1/sensor")]
    public class SensorController : ApiBase
    {
        private readonly ISensorService _sensorService;
        private readonly ILogger<SensorController> _logger;

        public SensorController(
            ISensorService sensorService,
            IAccountService accountService,
            ILogger<SensorController> logger) : base(accountService)
        {
            _sensorService = sensorService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddSensor(SensorAddRequest request)
        {
            try
            {
                return Ok(await _sensorService.AddSensorAsync(await CurrentUser, request));
            }
            catch (SensorAlreadyExistsException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }

        [HttpPut("{sensorId}")]
        public async Task<IActionResult> AddSensor(string sensorId, SensorUpdateRequest request)
        {
            try
            {
                return Ok(await _sensorService.EditSensorAsync(await CurrentUser, sensorId, request));
            }
            catch (SensorAlreadyExistsException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }
    }
}