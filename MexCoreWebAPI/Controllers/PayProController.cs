using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MexCoreWebAPI.Services;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayProController : ControllerBase
    {
        private readonly PayProService _authService;

        public PayProController(PayProService authService)
        {
            _authService = authService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] PayProRequest authRequest)
        //{
        //    var response = await _authService.AuthenticateAsync(authRequest);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        return Ok(responseContent);
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to authenticate");
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PayProRequest authRequest)
        {
            string token = await _authService.AuthenticateAsync(authRequest);

            if (token != null)
            {
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Token not found in response headers.");
            }
        }
    }
}
