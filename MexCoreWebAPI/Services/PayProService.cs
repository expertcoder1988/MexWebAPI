
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MexCoreWebAPI.Services
{

    public class PayProService
    {
        private readonly HttpClient _httpClient;

        public PayProService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AuthenticateAsync(PayProRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://demoapi.paypro.com.pk/v2/ppro/auth", content);

            // Check if the response has the 'Token' header
            if (response.Headers.TryGetValues("Token", out var tokens))
            {
                // Extract the first (or only) 'Token' value
                string token = tokens.FirstOrDefault();
                return token;
            }
            else
            {
                // Handle the case when 'Token' header is not found
                return null;
            }
        }
    }

    public class PayProRequest
    {
        public string clientid { get; set; }
        public string clientsecret { get; set; }
    }
}
