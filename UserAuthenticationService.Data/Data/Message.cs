using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? SentBy { get; set; }
        public string? Body { get; set; }
        public string? Base64 { get; set; }
        public DateTime? SentAt { get; set; }
        public int ApiUserId { get; set; }
        [ForeignKey("ApiUserId")]
        public ApiUser ApiUser { get; set; }
    }
}
