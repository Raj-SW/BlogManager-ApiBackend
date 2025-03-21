using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDKFxDC0dfSKJlYqKOvqCNU1sh_v1NvHHY"));

        [HttpPost("NativeRegister")]
        public async Task<IActionResult> NativeRegisterAsync(string email, string password)
        {
            try
            {
                var result = await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);
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
                var result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                return Ok(result);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("LoginByGoogle")]
        public async Task<IActionResult> LoginByGoogleAsync(string email, string password)
        {
            try
            {
                var result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                return Ok(result);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                return await Task.FromResult(Ok());
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
