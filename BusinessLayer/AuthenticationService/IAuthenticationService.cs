﻿using Firebase.Auth;

namespace BusinessLayer.AuthenthicationService
{
    public interface IAuthenticationService
    {
        Task<FirebaseAuthLink> NativeRegisterAsync(string email, string password);
        Task<FirebaseAuthLink> NativeLoginAsync(string email, string password);
        Task<FirebaseAuthLink> LoginByGoogleAsync(string email, string password);
        Task LogoutAsync();
    }
}
