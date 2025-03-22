using BusinessLayer.AuthenthicationService;
using Microsoft.AspNetCore.Mvc;
using Model.Utils;
using User = Model.User.User;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _config;

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration config)
        {
            _authenticationService = authenticationService;
            _config = config;
        }

        [HttpPost("NativeRegister")]
        public async Task<IActionResult> NativeRegisterAsync([FromBody] User userRegistrationDTO)
        {
            Result result = await _authenticationService.NativeRegisterAsync(userRegistrationDTO);
            return Ok(result);
        }

        [HttpPost("NativeLogin")]
        public async Task<IActionResult> NativeLoginAsync(string email, string password)
        {
            Result result = await _authenticationService.NativeLoginAsync(email, password);
            return Ok(result);
        }

        [HttpPost("LoginByGoogleAsync")]
        public async Task<IActionResult> LoginByGoogleAsync(string email, string password)
        {

            var result = await _authenticationService.LoginByGoogleAsync(email, password);
            return Ok(result);
        }

        [HttpPost("LogoutAsync")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authenticationService.LogoutAsync();
            return Ok();
        }
    }
}
