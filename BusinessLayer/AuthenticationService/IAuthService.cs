using Model.DTO.Authentication;
using Model.User;
using Model.Utils;


namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthService
    {
        Task<Result> NativeRegisterAsync(NativeSignUpDto nativeSignUpDTO);
        Task<GenericResult<User>> NativeLoginAsync(LoginDto loginDto);
    }
}
