using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sula.Api.Controllers
{
    public class AuthorizationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            try
            {
                return request.GrantType switch
                {
                    GrantTypes.Password => await HandlePasswordGrant(request),
                    GrantTypes.RefreshToken => await HandleTokenRefreshGrant(),
                    _ => BadRequest()
                };
            }
            catch (TokenRequestForbiddenException exception)
            {
                return Forbid(
                    new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = exception.Message
                    }), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
                );
            }
        }

        private async Task<IActionResult> HandlePasswordGrant(OpenIddictRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
 
            if (user?.PasswordHash == null)
            {
                throw new TokenRequestForbiddenException("The username/password couple is invalid.");
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                throw new TokenRequestForbiddenException("The username/password couple is invalid.");
            }

            var principal = await GetClaimsPrincipal(user);

            principal.SetScopes(new[]
            {
                Scopes.OpenId,
                Scopes.Email,
                Scopes.Profile,
                Scopes.OfflineAccess,
                Scopes.Roles
            }.Intersect(request.GetScopes()));

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> HandleTokenRefreshGrant()
        {
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var user = await _userManager.GetUserAsync(info.Principal);
            if (user == null)
            {
                throw new TokenRequestForbiddenException("The refresh token is no longer valid.");
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                throw new TokenRequestForbiddenException("The user is no longer allowed to sign in.");
            }

            var principal = await GetClaimsPrincipal(user);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<ClaimsPrincipal> GetClaimsPrincipal(ApplicationUser user)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            foreach (var claim in principal.Claims)
            {
                if (claim.Type != "AspNet.Identity.SecurityStamp")
                {
                    claim.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                }
            }

            return principal;
        }
    }
}