using Application.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Interface
{
    public  interface IAuthService
    {
        
        
        Task UpdateUserDetails(UpdateUserDetails input);
        Task ResetPassword(ResetPasswordDto input);
        Task ChangePassword(ChangePasswordDto input);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto input);
        Task<string> RefreshToken(string refreshToken);

    }
}
