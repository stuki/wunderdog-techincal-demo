using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sula.Core.Services.Interfaces;

namespace Sula.Api.Controllers
{
    [Route("api/v1/alerts")]
    public class AlertController : ApiBase
    {
        private readonly IAlertService _alertService;

        public AlertController(IAccountService accountService, IAlertService alertService) : base(accountService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlertsForUser()
        {
            return Ok( _alertService.GetAllAlertsForUser(await CurrentUser));
        }
    }    
}
