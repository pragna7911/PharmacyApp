using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using Newtonsoft.Json;
using System.Text;

namespace Wellgistics.Pharmacy.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration, IAuthService authService, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _authService = authService;
            _logger = logger;
        }
        [HttpGet("token")]
        public async Task<IActionResult> GetAuthTokenAsync(string AuthCode)
        {
            try
            {
                var tokenEndpoint = $"{_configuration.GetSection("Auth0:DomainName").Value}/oauth/token";
                var postData = new
                {
                    grant_type = "authorization_code",
                    client_id = _configuration.GetSection("Auth0:ClientId").Value,
                    client_secret = _configuration.GetSection("Auth0:ClientSecret").Value,
                    redirect_uri = _configuration.GetSection("Auth0:RedirectURL").Value,
                    code = AuthCode
                };
                Auth0Token response = await _authService.GetAuthTokenAsync(tokenEndpoint, postData);

                if (response != null)
                {
                    return Ok(response);
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
            

            return Unauthorized("Unable to retrieve token");
        }
        [HttpGet("signup")]
        //[Authorize(Policy = "ApiKeyPolicy")]
        //[Authorize(Policy = "Auth0JwtPolicy")]
        public async Task<IActionResult> SignUp(long ncpdp)
        {
            try
            {
                var tokenEndpoint = $"{_configuration.GetSection("Auth0:DomainName").Value}/dbconnections/signup";
                List<PharmacyEmployee> response = await _authService.Signup(ncpdp);
                if (response != null)
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(@"{status} {message} {exception} ", "", "Signup failed", ex.Message + " - " + ex.StackTrace);
                return StatusCode(500, ex.Message);
            }


            return Unauthorized("Unable to retrieve token");
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> GetTokenByRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                var tokenEndpoint = $"{_configuration.GetSection("Auth0:DomainName").Value}/oauth/token";
                var postData = new
                {
                    grant_type = "refresh_token",
                    client_id = _configuration.GetSection("Auth0:ClientId").Value,
                    refresh_token = refreshToken.refresh_token
                };
                Auth0Token response = await _authService.GetAuthTokenAsync(tokenEndpoint, postData);

                if (response != null)
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }


            return Unauthorized("Unable to retrieve token");
        }
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword(string emailId)
        {
            try
            {
                var tokenEndpoint = $"{_configuration.GetSection("Auth0:DomainName").Value}/dbconnections/change_password";
                var postData = new
                {
                    client_id = _configuration.GetSection("Auth0:ClientId").Value,
                    connection = _configuration.GetSection("Auth0:connection").Value,
                    email = emailId,
                    //redirectTo= _configuration.GetSection("Auth0:passwordChangeUrl").Value + "?email="+emailId,

                };
                string response = await _authService.ChangePassword(tokenEndpoint, postData);

                if (response != null)
                {
                    return Ok(new { status = "sucess", message= response });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }


            return Unauthorized("Unable to retrieve token");
        }

        [HttpPost("passwordStatus")]
        public async Task<IActionResult> PasswordStatus(UserPassword userPassword)
        {
            try
            {
                
                int response = await _authService.UpdatePasswordStatus(userPassword);
                Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }


            return Unauthorized("Unable to retrieve token");
        }



    }
}
