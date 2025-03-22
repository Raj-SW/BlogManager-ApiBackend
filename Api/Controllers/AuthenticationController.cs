using BusinessLayer.AuthenthicationService;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("NativeRegister")]
        public async Task<IActionResult> NativeRegisterAsync(string email, string password)
        {
            try
            {
                var result = await _authenticationService.NativeRegisterAsync(email, password);
                return Ok(result);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("NativeLogin")]
        public async Task<IActionResult> NativeLoginAsync(string email, string password)
        {
            try
            {
                var result = await _authenticationService.NativeLoginAsync(email, password);
                return Ok(result);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("LoginByGoogleAsync")]
        public async Task<IActionResult> LoginByGoogleAsync(string email, string password)
        {
            try
            {
                var result = await _authenticationService.LoginByGoogleAsync(email, password);
                return Ok(result);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("LogoutAsync")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _authenticationService.LogoutAsync();
                return Ok();
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
