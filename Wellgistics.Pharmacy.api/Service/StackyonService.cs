using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;

public class StackyonService : IStackyonService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<StackyonService> _logger;
    private string _token;
    private DateTime _tokenExpiration;
    private readonly IServiceLogService _serviceLog;

    public StackyonService(IConfiguration configuration, ILogger<StackyonService> logger, IServiceLogService serviceLog)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceLog = serviceLog;
    }

    public async Task<StackyonConnectResponse> CallServiceApi(object requestBody)
    {
        try
        {
            string token = await GetTokenAsync();
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync($"{_configuration.GetSection("stackyon-connect:baseUrl").Value}v1/Service/connect", requestContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Token might be expired, generate a new token and retry
                    token = await GenerateNewTokenAsync();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    response = await client.PostAsync($"{_configuration.GetSection("stackyon-connect:baseUrl").Value}v1/Service/connect", requestContent);
                }
                string encodedUrl = "";
                var responseContent = await response.Content.ReadAsStringAsync();
                var propertyInfo = requestBody.GetType().GetProperty("encodedUrl");
                if (propertyInfo != null)
                {
                    encodedUrl = (string)propertyInfo.GetValue(requestBody);
                }
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(@"{status} {message} {exception} ", response.StatusCode, "StackyonServiceCall failed", responseContent);
                    await _serviceLog.LogServiceCallAsync(new ServiceLogModel
                    {
                        DestinationService = "Stackyon-Connect",
                        SourceServiceName = "PharmacyApi",
                        EnCodedUrl = encodedUrl,
                        MethodType = "Post",
                        RequestBody = JsonConvert.SerializeObject(requestBody),
                        RequestUri = "api/v1/Service/connect",
                        StatusCode = (int)response.StatusCode,
                        IsActive = true,
                        ResponseBody = responseContent
                    });
                    return new StackyonConnectResponse { StatusCode = 500, Message = responseContent };
                    //throw new Exception("Failed to call service API.");
                }
                else if (response.IsSuccessStatusCode)
                {


                    
                    await _serviceLog.LogServiceCallAsync(new ServiceLogModel
                    {
                        DestinationService = "Stackyon-Connect",
                        SourceServiceName = "PharmacyApi",
                        EnCodedUrl = encodedUrl,
                        MethodType = "Post",
                        RequestBody = JsonConvert.SerializeObject(requestBody),
                        RequestUri = "api/v1/Service/connect",
                        StatusCode = (int)response.StatusCode,
                        IsActive = true,
                        ResponseBody = responseContent
                    });
                    return new StackyonConnectResponse { StatusCode = 200, Message = responseContent };
                }
                else
                {
                    return new StackyonConnectResponse { StatusCode = 500, Message = "Internal Error While Connecting.." };
                }
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(@"{status} {message} {exception} ", "", "StackyonServiceCall failed", ex.Message + " - " + ex.StackTrace);
            return new StackyonConnectResponse { StatusCode = 500, Message = "Internal Error While Connecting.." }; ;
        }
        

        
    }

    private async Task<string> GenerateNewTokenAsync()
    {
        var tokenRequestBody = new
        {
            name = _configuration.GetSection("stackyon-connect:name").Value,
            clientId = _configuration.GetSection("stackyon-connect:clientId").Value,
            clientSecret = _configuration.GetSection("stackyon-connect:clientSecret").Value
        };

        var requestContent = new StringContent(JsonConvert.SerializeObject(tokenRequestBody), Encoding.UTF8, "application/json");

        using (var client = new HttpClient())
        {
            var response = await client.PostAsync($"{_configuration.GetSection("stackyon-connect:baseUrl").Value}v1/Authorize/generate-token", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to generate token. Status Code: {StatusCode}", response.StatusCode);
                throw new Exception("Failed to generate token.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            _token = tokenResponse.Token;
            _tokenExpiration = DateTime.UtcNow.AddHours(10);
        }

        return _token;
    }

    private async Task<string> GetTokenAsync()
    {
        if (string.IsNullOrEmpty(_token) || _tokenExpiration <= DateTime.UtcNow)
        {
            return await GenerateNewTokenAsync();
        }

        return _token;
    }

    public async Task<string> Decrypt(string encodedUrl)
    {
        string token = await GetTokenAsync();

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"{_configuration.GetSection("stackyon-connect:baseUrl").Value}v1/Service/decrypt", null);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Token might be expired, generate a new token and retry
                token = await GenerateNewTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                response = await client.PostAsync($"{_configuration.GetSection("stackyon-connect:baseUrl").Value}v1/Service/decrypt", null);
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(@"{status} {message} {exception} ", response.StatusCode, "StackyonDecriptServiceCall failed", responseContent);
                return "";
                //throw new Exception("Failed to call service API.");
            }
            else if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ApiResponse>(responseContent)?.data?.sid;
            }
            else
            {
                return "";
            }
        }
    }

    private class TokenResponse
    {
        public string Token { get; set; }
    }
    public class StackyonConnectResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class DecryptResponseData
    {
        public string wsid { get; set; }
        public string sid { get; set; }
        public string srtid { get; set; }
        public string wssid { get; set; }
        public string dynamic { get; set; }
        public string dataObjectTypeId { get; set; }
        public string dorid { get; set; }
    }

    public class ApiResponse
    {
        public int code { get; set; }
        public DecryptResponseData data { get; set; }
    }



}
