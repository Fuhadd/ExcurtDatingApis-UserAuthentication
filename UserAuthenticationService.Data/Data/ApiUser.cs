using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Data
{
    public class ApiUser : IdentityUser<int>
    {
        
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Hash { get; set; }
        public string? MobileNumberDialCode { get; set; }
        public string? MobileNumber { get; set; }
        public int OnboardingStep { get; set; }
        public int Likeability { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Interests { get; set; }
        public virtual List<UserImage>? UserImages { get; set; }
        public virtual List<Message>? Messages { get; set; }

        public List<UserMatch> MatchesSent { get; set; }

        public List<UserMatch> MatchesReceieved { get; set; }

    }
}
