using BusinessLayer.AuthenthicationService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.DTO.Authentication;
using Model.User;
using Model.Utils;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IConfiguration _config;

        public AuthenticationController(IAuthService authenticationService, IConfiguration config)
        {
            _authenticationService = authenticationService;
            _config = config;
        }

        [HttpPost("NativeRegister")]
        public async Task<IActionResult> NativeRegisterAsync([FromBody] NativeSignUpDto nativeSignUpDto)
        {
            Result result = await _authenticationService.NativeRegisterAsync(nativeSignUpDto);
            return Ok(result);
        }

        [HttpPost("NativeLogin")]
        public async Task<IActionResult> NativeLoginAsync([FromBody] LoginDto loginDto)
        {
            GenericResult<User> result = await _authenticationService.NativeLoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("LogoutAsync")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
