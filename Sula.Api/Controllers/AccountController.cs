using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sula.Core.Exceptions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Response;
using Sula.Core.Services.Interfaces;

namespace Sula.Api.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : ApiBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, IUserService userService, ILogger<AccountController> logger) : base(accountService)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userResponse = new UserResponse(await CurrentUser);

                return Ok(userResponse);
            }
            catch (UserNotFoundException exception)
            {
                _logger.LogError(exception, exception.Code);
                return BadRequest(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ApplicationUser userUpdate)
        {
            try
            {
                var user = await CurrentUser;

                user.Update(userUpdate);
                
                var userResponse = await _userService.UpdateAsync(user);

                return Ok(userResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest(exception);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("forgot")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await _userService.SendPasswordResetToken(request.Email);

                return Ok();
            }
            catch (Exception exception)
            {
                if (exception is UserNotFoundException)
                {
                    _logger.LogInformation("User failed to start request email request process with email: " + request.Email);
                }
                
                return Ok();
            }
        }
    }
}