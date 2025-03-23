using Model.DTO.Authentication;
using Model.Utils;


namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthenticationService
    {
        Task<Result> NativeRegisterAsync(NativeSignUpDto nativeSignUpDTO);
        Task<Result> NativeLoginAsync(LoginDto loginDto);
        Task<Result> LoginByGoogleAsync(LoginDto loginDto);
        Task LogoutAsync();
    }
}
