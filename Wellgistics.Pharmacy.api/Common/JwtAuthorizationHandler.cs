//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace Wellgistics.Pharmacy.api.Common
//{
//    public class JwtAuthorizationHandler : AuthorizationHandler<JwtRequirement>
//    {
//        private readonly IConfiguration _configuration;
//        private static readonly HttpClient _httpClient = new HttpClient();

//        public JwtAuthorizationHandler(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        protected override async  Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
//        {
//            // Access HttpContext via context.Resource
//            if (context.Resource is not HttpContext httpContext)
//            {
//                return ;
//            }
//            var jwtToken = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//            if (string.IsNullOrEmpty(jwtToken))
//            {
//                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                await httpContext.Response.WriteAsync("Unauthorized: JWT token is required.");
//                return ;
//            }

//            if (await ValidateJwtTokenAsync(jwtToken, httpContext))  // Validate JWT token
//            {
//                context.Succeed(requirement);  // Authorization succeeded
//            }
//            else
//            {
//                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                context.Fail();
//                 //await httpContext.Response.WriteAsync("Unauthorized: Invalid API Key.");
//                //return;
//            }
//        }

//        private async Task<bool> ValidateJwtTokenAsync(string token, HttpContext httpContext)
//        {
//            var issuer = _configuration["Auth0:DomainName"]+"/";
//            var audience = _configuration["Auth0:ClientId"];
//            var domain = _configuration["Auth0:DomainName"];  // Auth0 Domain for the issuer

//            try
//            {
//                // Fetch the public keys from Auth0
//                var jwks = await GetAuth0PublicKeysAsync(domain);

//                // Extract the "kid" (key ID) from the JWT header
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
//                var kid = jsonToken?.Header["kid"]?.ToString();

//                if (string.IsNullOrEmpty(kid))
//                {
//                    return false;  // Invalid or missing key ID in the token header
//                }

//                // Find the public key with the matching kid
//                var publicKey = jwks?.Keys.FirstOrDefault(key => key.Kid == kid)?.PublicKey;
//                if (publicKey == null)
//                {
//                    return false;  // Public key not found in Auth0's JWKS for this kid
//                }

//                var validationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidIssuer = issuer,
//                    ValidAudience = audience,
//                    IssuerSigningKey = publicKey
//                };

//                // Validate the token using the public key
//                var validatedToken = tokenHandler.ValidateToken(token, validationParameters, out var validatedJwtToken);

//                // If valid, set the claims into the HttpContext.User
//                if (validatedJwtToken is JwtSecurityToken jwtToken)
//                {
//                    var claims = jwtToken.Claims.ToList();

//                    // Set the claims to the HttpContext.User for controller access
//                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

//                    return true;
//                }

//                return false;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Fetch the public keys from Auth0's JWKS endpoint
//        private async Task<JWKSet> GetAuth0PublicKeysAsync(string domain)
//        {
//            try
//            {
//                var jwksUrl = $"{domain}/.well-known/jwks.json";
//                var response = await _httpClient.GetStringAsync(jwksUrl);
//                var jwks = JsonConvert.DeserializeObject<JWKSet>(response);
//                return jwks;
//            }
//            catch (HttpRequestException e)
//            {
//                // Log the exception or handle it as necessary
//                Console.WriteLine($"HTTP Request failed: {e.Message}");
//                return null;
//            }
//        }
//    }

//    // Representation of a JSON Web Key Set (JWKS)
//    public class JWKSet
//    {
//        public List<JWK> Keys { get; set; }
//    }

//    // Representation of a single JSON Web Key
//    public class JWK
//    {
//        public string Kid { get; set; }
//        public string Kty { get; set; }
//        public string Alg { get; set; }
//        public string N { get; set; }  // Modulus
//        public string E { get; set; }  // Exponent

//        public SecurityKey PublicKey
//        {
//            get
//            {
//                var rsaParameters = new RSAParameters
//                {
//                    Modulus = Base64UrlDecode(N),
//                    Exponent = Base64UrlDecode(E)
//                };

//                return new RsaSecurityKey(rsaParameters) { KeyId = Kid };
//            }
//        }

//        private static byte[] Base64UrlDecode(string input)
//        {
//            var padding = input.Length % 4 == 0 ? 0 : 4 - input.Length % 4;
//            input = input + new string('=', padding);
//            return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
//        }
//    }

//    public class JwtRequirement : IAuthorizationRequirement { }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wellgistics.Pharmacy.api.Common
{
    public class JwtAuthorizationHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static DateTime _lastFetchedTime = DateTime.MinValue;
        private static JWKSet _cachedJwks = null;
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);
        public JwtAuthorizationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            // Access HttpContext via context.Resource
            if (context.Resource is not HttpContext httpContext)
            {
                return;
            }
            var jwtToken = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(jwtToken))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized: JWT token is required.");
                return;
            }

            if (await ValidateJwtTokenAsync(jwtToken, httpContext))  // Validate JWT token
            {
                context.Succeed(requirement);  // Authorization succeeded
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Fail();
                //await httpContext.Response.WriteAsync("Unauthorized: Invalid API Key.");
                //return;
            }
        }

        private async Task<bool> ValidateJwtTokenAsync(string token, HttpContext httpContext)
        {
            var issuer = _configuration["Auth0:DomainName"] + "/";
            var audience = _configuration["Auth0:ClientId"];
            var domain = _configuration["Auth0:DomainName"];  // Auth0 Domain for the issuer

            try
            {
                JWKSet jwks;
                if (_cachedJwks == null || DateTime.UtcNow - _lastFetchedTime > _cacheDuration)
                {
                    // Fetch the public keys from Auth0
                    jwks = await GetAuth0PublicKeysAsync(domain ?? "");
                    if (jwks != null)
                    {
                        _cachedJwks = jwks;
                        _lastFetchedTime = DateTime.UtcNow;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    jwks = _cachedJwks;
                }
                
                //var jwks = await GetAuth0PublicKeysAsync(domain);

                // Extract the "kid" (key ID) from the JWT header
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var kid = jsonToken?.Header["kid"]?.ToString();

                if (string.IsNullOrEmpty(kid))
                {
                    return false;  // Invalid or missing key ID in the token header
                }

                // Find the public key with the matching kid
                var publicKey = jwks?.Keys.FirstOrDefault(key => key.Kid == kid)?.PublicKey;
                if (publicKey == null)
                {
                    return false;  // Public key not found in Auth0's JWKS for this kid
                }

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = publicKey
                };

                // Validate the token using the public key
                var validatedToken = tokenHandler.ValidateToken(token, validationParameters, out var validatedJwtToken);

                // If valid, set the claims into the HttpContext.User
                if (validatedJwtToken is JwtSecurityToken jwtToken)
                {
                    var claims = jwtToken.Claims.ToList();

                    // Set the claims to the HttpContext.User for controller access
                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Fetch the public keys from Auth0's JWKS endpoint
        private async Task<JWKSet> GetAuth0PublicKeysAsync(string domain)
        {
            try
            {
                var jwksUrl = $"{domain}/.well-known/jwks.json";
                var response = await _httpClient.GetStringAsync(jwksUrl);
                var jwks = JsonConvert.DeserializeObject<JWKSet>(response);
                return jwks;
            }
            catch (HttpRequestException e)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine($"HTTP Request failed: {e.Message}");
                return null;
            }
        }
    }

    // Representation of a JSON Web Key Set (JWKS)
    public class JWKSet
    {
        public List<JWK> Keys { get; set; }
    }

    // Representation of a single JSON Web Key
    public class JWK
    {
        public string Kid { get; set; }
        public string Kty { get; set; }
        public string Alg { get; set; }
        public string N { get; set; }  // Modulus
        public string E { get; set; }  // Exponent

        public SecurityKey PublicKey
        {
            get
            {
                var rsaParameters = new RSAParameters
                {
                    Modulus = Base64UrlDecode(N),
                    Exponent = Base64UrlDecode(E)
                };

                return new RsaSecurityKey(rsaParameters) { KeyId = Kid };
            }
        }

        private static byte[] Base64UrlDecode(string input)
        {
            var padding = input.Length % 4 == 0 ? 0 : 4 - input.Length % 4;
            input = input + new string('=', padding);
            return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
        }
    }

    public class JwtRequirement : IAuthorizationRequirement { }
}

