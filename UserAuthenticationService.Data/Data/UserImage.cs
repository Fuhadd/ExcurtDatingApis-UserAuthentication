using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Data
{
    public class UserImage
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? ImageSize { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ApiUserId { get; set; }
        [ForeignKey("ApiUserId")]
        public ApiUser ApiUser { get; set; }
    }
}
