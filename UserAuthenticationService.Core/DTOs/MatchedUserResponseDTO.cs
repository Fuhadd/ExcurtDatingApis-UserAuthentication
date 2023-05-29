using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.DTOs
{
    public class MatchedUserResponseDTO
    {
        public ApiUser User { get; set; }
        public double Similarity { get; set; }
    }
}
