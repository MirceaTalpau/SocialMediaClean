﻿using LinkedFit.DOMAIN.Models.DTOs.Auth;

namespace SocialMediaClean.PERSISTANCE.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUserAsync(RegisterRequestDTO user,string confirmationToken);
        Task<PasswordAndSalt> GetPasswordAndSaltAsync(string email);
        Task<bool> IsEmailVerifiedAsync(string email);
        Task<CheckGmailDTO> CheckGmailAsync(string email);


    }
}
