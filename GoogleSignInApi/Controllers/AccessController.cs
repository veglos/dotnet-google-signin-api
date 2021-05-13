using Google.Apis.Auth;
using GoogleSignInApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GoogleSignInApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IOptions<AppSettings> _settings;

        public AccessController(IOptions<AppSettings> settings)
        {
            _settings = settings;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin-google")]
        public async Task<string> SignInGoogle(GoogleSignInTokenRequest request)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[] { _settings.Value.GoogleSettings.ClientID }
                };

                var token = request.IdToken;
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                // Now we are sure the user has authenticated via Google and we can proceed to do anything
                // like fetch the user's claims and provide her/him with an Access Token.
                return "OK";
            }
            catch (InvalidJwtException ex)
            {
                return $"ERROR: InvalidJwt - {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signout-google")]
        public async Task<string> SignOutGoogle() //should actually receive a request with the access token and the user Id.
        {
            try
            {
                // Proceed to take any actions if needed such as disabling or deleting the user's refresh token.
                return "OK";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }
}
