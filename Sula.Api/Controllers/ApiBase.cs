using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Services.Interfaces;

namespace Sula.Api.Controllers
{
    public class ApiBase : ControllerBase
    {
        protected readonly IAccountService AccountService;

        protected Task<ApplicationUser> CurrentUser => AccountService.GetUserAsync(User);

        public ApiBase(IAccountService accountService)
        {
            AccountService = accountService;
        }
    }
}