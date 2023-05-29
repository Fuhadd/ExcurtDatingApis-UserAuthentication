using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.DTOs
{
    public class UserImageDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? ImageSize { get; set; }
    }
}
