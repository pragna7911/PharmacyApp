using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Wellgistics.Pharmacy.api.Common
{
    public class ApiKeyAuthorizationHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        private readonly string _apiKey;

        public ApiKeyAuthorizationHandler(IConfiguration configuration)
        {
            // Read API Key from appsettings.json
            _apiKey = configuration["ApiSettings:ApiKey"];
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            if (context.Resource is not HttpContext httpContext)
            {
                return;
            }
            try
            {
                await Task.Run(() =>
                {
                    var apiKey = httpContext.Request.Headers["Authorization"].FirstOrDefault();

                    if (!string.IsNullOrEmpty(apiKey) || apiKey == _apiKey)
                    {
                        context.Succeed(requirement);
                        //return Task.CompletedTask;

                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Fail();
                    }
                });

                

                
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred during API Key authentication.");
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Fail();
            }
        }
    }

    public class ApiKeyRequirement : IAuthorizationRequirement { }

}
