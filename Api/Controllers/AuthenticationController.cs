using BusinessLayer.AuthenthicationService;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var result = await _authenticationService.NativeRegisterAsync(userRegistrationDTO);
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
            if (email == null || password == null)
                return BadRequest("Invalid login data.");
            try
            {
                FirebaseAuthLink result = await _authenticationService.NativeLoginAsync(email, password);
                if (result != null)
                {
                    var secretKey = _config["Jwt:SecretKey"];
                    var token = JwtManager.GenerateToken(
                       userId: result.User.LocalId,
                       role: "LoggedUser",
                       secretKey: secretKey
                   );
                    return Ok(new { authResult = result, token });
                }
                return Unauthorized(new { error = "Authentication failed." });
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
