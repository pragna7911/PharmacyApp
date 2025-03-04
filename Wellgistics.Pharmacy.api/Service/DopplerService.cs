using Wellgistics.Pharmacy.api.IService;

namespace Wellgistics.Pharmacy.api.Service
{
    public class DopplerService : IDopplerService
    {
        private readonly string _dopplerApiKey;
        private readonly HttpClient _httpClient;

        public DopplerService(IConfiguration configuration, HttpClient httpClient)
        {
            _dopplerApiKey = configuration["Doppler:ApiKey"];
            _httpClient = httpClient;
        }

        public async Task<string> GetSecretValueAsync(string secretKey)
        {
            var url = $"https://api.doppler.com/v3/configs/secrets/data?key={secretKey}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Authorization", $"Bearer {_dopplerApiKey}" }
                },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                // Deserialize the response if necessary (optional)
                return responseBody;
            }
        }
    }
}
