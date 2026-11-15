using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayfastController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PayfastController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken([FromBody] AuthRequest authRequest)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://ipg1.apps.net.pk/Ecommerce/api/Transaction/GetAccessToken";

            var requestContent = new StringContent(JsonSerializer.Serialize(authRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<TokenResponse>(responseContent); // Assuming the response can be deserialized into this model
                    return Ok(token);
                }

                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Request to external service failed: {ex.Message}");
            }
        }

        [HttpPost("getaccesstokenuat")]
        public async Task<IActionResult> GetAccessTokenUat([FromBody] AuthRequest authRequest)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://ipguat.apps.net.pk/Ecommerce/api/Transaction/GetAccessToken";

            var requestContent = new StringContent(JsonSerializer.Serialize(authRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<TokenResponse>(responseContent); // Assuming the response can be deserialized into this model
                    return Ok(token);
                }

                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Request to external service failed: {ex.Message}");
            }
        }


        [HttpPost("request")]
        public async Task<IActionResult> ProcessPaymentRequest([FromBody] TransactionRequest request)
        {
            string PaymentApiUrl = "https://ipguat.apps.net.pk/Ecommerce/api/Transaction/PostTransaction";

            if (request == null)
            {
                return BadRequest("Invalid payment request data.");
            }

           // string token = await GetAccessToken();

            if (string.IsNullOrEmpty(request.TOKEN))
            {
                return StatusCode(500, "Failed to obtain access token.");
            }

            // Generate signature
            string signature = GenerateSignature(request.MERCHANT_ID, request.TXNAMT.ToString(), request.BASKET_ID);

            // Prepare data for payment request
            var paymentData = new Dictionary<string, string>
            {
                { "CURRENCY_CODE", "PKR" },
                { "MERCHANT_ID", request.MERCHANT_ID.ToString() },
                { "TOKEN", request.TOKEN },
                { "SUCCESS_URL", "https://malikexpress.com" },
                { "FAILURE_URL", "https://malikexpress.com" },
                { "CHECKOUT_URL", "https://typedwebhook.tools/webhook/cfe4e40e-8c5c-4d5b-867a-017bce41070c" },
                { "CUSTOMER_EMAIL_ADDRESS", "asimshahzad78@gmail.com" },
                { "TXNAMT", request.TXNAMT.ToString() },
                { "BASKET_ID", request.BASKET_ID },
                { "ORDER_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                { "SIGNATURE", signature },
                { "VERSION", "MERCHANT-CART-0.1" },
                { "TXNDESC", "Item Purchased from Cart" },
                { "PROCCODE", "00" },
                { "TRAN_TYPE", "ECOMM_PURCHASE" },
                { "STORE_ID", "" }, // Optional
                { "RECURRING_TXN", "true" }
            };

            // Make payment request
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(PaymentApiUrl, new FormUrlEncodedContent(paymentData));

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Ok(responseData); // or process the response as needed
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Payment request failed.");
                }
            }
        }

        private string GenerateSignature(string MerchantId, string transactionAmount, string basketId)
        {
            string secretWord = ""; // Obtain secret word from merchant portal
            string responseString = $"{MerchantId}{basketId}{secretWord}{transactionAmount}";
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(responseString));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class TransactionRequest
    {
        public string CURRENCY_CODE { get; set; } = "PKR";
        public string MERCHANT_ID { get; set; }
        public string TOKEN { get; set; }
        public string SUCCESS_URL { get; set; } = "http://merchant-site-example.com";
        public string FAILURE_URL { get; set; } = "http://merchant-site-example.com";
        public string CHECKOUT_URL { get; set; } = "https://typedwebhook.tools/webhook/cfe4e40e-8c5c-4d5b-867a-017bce41070c";
        public string CUSTOMER_EMAIL_ADDRESS { get; set; } = "some-email@example.com";
        public decimal TXNAMT { get; set; }
        public string BASKET_ID { get; set; }
        public DateTime ORDER_DATE { get; set; } = DateTime.Now;
        public string SIGNATURE { get; set; }
        public string VERSION { get; set; } = "MERCHANT-CART-0.1";
        public string TXNDESC { get; set; } = "Item Purchased from Cart";
        public string PROCCODE { get; set; } = "00";
        public string TRAN_TYPE { get; set; } = "ECOMM_PURCHASE";
        public string STORE_ID { get; set; } = string.Empty;
        public bool RECURRING_TXN { get; set; } = true;
    
    }

    public class AuthRequest
    {
        public string merchant_id { get; set; }
        public string secured_key { get; set; }
    public string CURRENCY_CODE { get; set; }
    public string TXNAMT { get; set; }
    public string BASKET_ID { get; set; }
    // Add other properties as per the expected request data
}
    public class TokenResponse
    {
        public string ACCESS_TOKEN { get; set; }
    }
}
