using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Models.Response
{
    public class RegisterEmailResponse
    {
        public RefreshTokenResponse? JwtToken { get; set; }
        public string? Email { get; set; }
        public int? OnboardingStep { get; set; }  
    }
}
