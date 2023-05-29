using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Models
{
    public class RefreshTokenResponse
    {
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public List<string>? Errors { get; set; }

        public bool Success { get; set; }
    }
}
