using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sula.Core.Exceptions;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;

namespace Sula.Api.Controllers
{
    [Route("api/v1/sensor/data")]
    public class SensorDataController : ApiBase
    {
        private readonly ISensorDataService _sensorDataService;
        private readonly ILogger<SensorController> _logger;

        public SensorDataController(
            ISensorDataService sensorDataService,
            IAccountService accountService,
            ILogger<SensorController> logger) : base(accountService)
        {
            _sensorDataService = sensorDataService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensorsAndPureData()
        {
            try
            {
                return Ok(await _sensorDataService.GetSensorsAndPureDataAsync(await CurrentUser));
            }
            catch (ExceptionWithCode exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorAndPureData(string id)
        {
            try
            {
                return Ok(await _sensorDataService.GetSensorAndPureDataAsync(await CurrentUser, id));
            }
            catch (ExceptionWithCode exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet("minmax")]
        public async Task<IActionResult> GetSensorsAndMinMaxData()
        {
            try
            {
                return Ok(await _sensorDataService.GetSensorsAndMinMaxDataAsync(await CurrentUser));
            }
            catch (ExceptionWithCode exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}