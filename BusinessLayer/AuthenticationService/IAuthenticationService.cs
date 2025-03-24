using Model.DTO.Authentication;
using Model.User;
using Model.Utils;


namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthenticationService
    {
        Task<Result> NativeRegisterAsync(NativeSignUpDto nativeSignUpDTO);
        Task<GenericResult<User>> NativeLoginAsync(LoginDto loginDto);
        Task<GenericResult<User>> LoginByGoogleAsync(LoginDto loginDto);
        Task LogoutAsync();
    }
}
