using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Services.Authentication
{
    public interface IAuthentication
    {
        Task<bool> AuthenticateAsync(LoginUserDTO user);

        Task<RefreshTokenResponse> CreateToken(ApiUser _user);
    }
}
