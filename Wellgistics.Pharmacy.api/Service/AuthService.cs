using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IRepositries;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using Wellgistics.Pharmacy.api.Repository;

namespace Wellgistics.Pharmacy.api.Service
{
    public class AuthService:IAuthService
    {
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly PharmacyDbContext _context;
        private readonly ILogger<AuthService> _logger;
        public AuthService(HttpClientHelper httpClientHelper, IConfiguration configuration, IRepository repository, IEmailService emailService, PharmacyDbContext context, ILogger<AuthService> logger) 
        {
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            _repository = repository;
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public async Task<Auth0Token> GetAuthTokenAsync(string url, object postData)
        {
            try
            {
                var response = await _httpClientHelper.SendRequestAsync<Auth0Token>(HttpMethod.Post, url, postData, "application/x-www-form-urlencoded");
                return response is not null && response.access_token is not null ? response : Activator.CreateInstance<Auth0Token>();
            }
            catch
            {
                throw;
            }

        }
        public async Task<List<PharmacyEmployee>> Signup(long ncpdp)
        {
            try
            {
                //get the Pharmacy employees by the pharmacyId
                
                var tokenEndpoint = $"{_configuration.GetSection("Auth0:DomainName").Value}/dbconnections/signup";
                List<PharmacyEmployee> users = await _repository.GetAllAsync<PharmacyEmployee>("EXEC GetEmployeesByncpdp @ncpdp", _context,
                            SqlHelper.CheckNotNullLong("@ncpdp", ncpdp)
                );
                foreach (PharmacyEmployee user in users) 
                {
                    string generatedPassword = PasswordGenerator.GeneratePasswordFromEmail(user.email);
                    var postData = new
                    {
                        client_id = _configuration.GetSection("Auth0:ClientId").Value,
                        email = user.email,
                        password = generatedPassword,
                        connection = _configuration.GetSection("Auth0:connection").Value
                    };
                    try
                    {
                        var response = await _httpClientHelper.SendRequestAsync<Auth0SignupResponse>(HttpMethod.Post, tokenEndpoint, postData);
                        if (response!.email is not null)
                        {
                            _logger.LogInformation("{status} {ApiName} {message}", "200", "/Auth0Signup", "Servie Log Sucess");
                            await _repository.ExecuteScalarAsync("EXEC UpdateEmployeeAccountStatus @email", _context,
                            SqlHelper.CheckNotNull("@email", user.email));
                            string body = await System.IO.File.ReadAllTextAsync("EmailTemplates/AccountDetails.html");
                            body = body.Replace("{username}", user.lastName).Replace("{Password}", generatedPassword);
                            await _emailService.SendEmail(user.email, "Pharmacy Application Account Details", body);
                            user.password = generatedPassword;
                        }
                    }
                    catch(Exception ex) 
                    {
                        _logger.LogError(@"{status} {message} {exception} ", "", "Signup AuthService failed", ex.Message + " - " + ex.StackTrace);

                    }
                    
                    
                }
                return users;
            }
            catch
            {
                throw;
            }

        }

        public async Task<string> ChangePassword(string url, object postData)
        {
            var response = await _httpClientHelper.SendRequestAsync<string>(HttpMethod.Post, url, postData);
            return response is not null ?  response : "";
        }
        public async Task<int> UpdatePasswordStatus(UserPassword userPassword)
        {
            try
            {
                return await _repository.ExecuteScalarAsync("EXEC UpdateEmployeePasswordStatus @email", _context,
                             SqlHelper.CheckNotNull("@email", userPassword.email));
            }
            catch
            {
                throw;
            }
        }
    }
}
