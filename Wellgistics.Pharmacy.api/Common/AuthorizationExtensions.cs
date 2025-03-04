using Microsoft.AspNetCore.Authorization;

namespace Wellgistics.Pharmacy.api.Common
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorizationPolicies(this IServiceCollection services)
        {
            // Register the API Key Authorization Handler
            services.AddSingleton<IAuthorizationHandler, ApiKeyAuthorizationHandler>();

            // Register the JWT Authorization Handler
            services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();

            // Add policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiKeyPolicy", policy =>
                    policy.Requirements.Add(new ApiKeyRequirement()));

                options.AddPolicy("Auth0JwtPolicy", policy =>
                    policy.Requirements.Add(new JwtRequirement()));
            });

            return services;
        }
    }

}
