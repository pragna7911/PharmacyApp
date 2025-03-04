using Newtonsoft.Json;
using System.Text;
using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.Common
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientHelper> _logger;

        public HttpClientHelper(HttpClient httpClient, ILogger<HttpClientHelper> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<T> SendRequestAsync<T>(HttpMethod method, string url, object data = null, string contentType = "")
        {
            var request = new HttpRequestMessage(method, url);

            // Check if data is provided and method is POST or PUT
            if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put))
            {
                // Check if we need to send data as x-www-form-urlencoded


                // If the data is of type 'x-www-form-urlencoded', format accordingly
                if (contentType == "application/x-www-form-urlencoded")
                {
                    // Convert the data to FormUrlEncodedContent
                    var formData = new List<KeyValuePair<string, string>>();

                    foreach (var property in data.GetType().GetProperties())
                    {
                        var value = property.GetValue(data)?.ToString();
                        formData.Add(new KeyValuePair<string, string>(property.Name, value));
                    }

                    var content = new FormUrlEncodedContent(formData);
                    request.Content = content;
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                }
                else
                {
                    // For JSON requests (default case)
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    request.Content = content;
                }
            }
            try
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    
                    // Ensure a successful status code (2xx)
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Failed to call service API. Status Code: {StatusCode}, Response: {Response}", response.StatusCode, responseContent);
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        // Check if T is a string (handle string responses differently)
                        if (typeof(T) == typeof(string))
                        {
                            return (T)(object)responseBody;
                        }
                        return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                        
                    
                    return Activator.CreateInstance<T>();

                    // Otherwise, try to deserialize the response to T

                }
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }
        public async Task<string> GetRequestAsync(string url)
        {
            
            var response = await _httpClient.GetAsync(url);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return "Error";
            //}
            response.EnsureSuccessStatusCode();
            // Return the response content as string (you could deserialize it if needed)
            return await response.Content.ReadAsStringAsync();
        }
    }
}
