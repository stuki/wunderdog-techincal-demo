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
    [Route("api/v1/sensor/{sensorId}/limit")]
    public class SensorLimitController : ApiBase
    {
        private readonly ISensorLimitService _sensorLimitService;
        private readonly ILogger<SensorController> _logger;

        public SensorLimitController(
            ISensorLimitService sensorLimitService,
            IAccountService accountService,
            ILogger<SensorController> logger) : base(accountService)
        {
            _sensorLimitService = sensorLimitService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string sensorId, SensorLimitAddRequest request)
        {
            try
            {
                return Ok(await _sensorLimitService.AddLimitAsync(await CurrentUser, sensorId, request));
            }
            catch (SensorDoNotExistException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }

        [HttpPut("{limitId}")]
        public async Task<IActionResult> Edit(string sensorId, int limitId, SensorLimitUpdateRequest request)
        {
            try
            {
                return Ok(await _sensorLimitService.EditLimitAsync(await CurrentUser, sensorId, limitId, request));
            }
            catch (SensorDoNotExistException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }

        [HttpDelete("{limitId}")]
        public async Task<IActionResult> Delete(string sensorId, int limitId)
        {
            try
            {
                return Ok(await _sensorLimitService.DeleteLimitAsync(await CurrentUser, sensorId, limitId));
            }
            catch (SensorDoNotExistException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }
    }
}