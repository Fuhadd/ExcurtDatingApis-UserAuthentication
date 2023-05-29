using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models;

namespace UserAuthenticationService.Core.Services.RefreshTokenServices
{
    public interface IRefreshToken
    {
        public Task<RefreshTokenResponse> VerifyAndGenerateRefreshTokenAsync(TokenRequestDTO tokenRequest);
    }
}
