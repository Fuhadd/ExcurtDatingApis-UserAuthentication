using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Data
{
    public class UserMatch
    {
        public int Id { get; set; }
        public double? similarities { get; set; }
        public bool HasChatted { get; set; }
        public bool IsLike { get; set; }
        public bool IsDislike { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? SentById { get; set; }
        [ForeignKey("SentById")]
        public ApiUser SentBy { get; set; }

        public int? SentToId { get; set; }
        [ForeignKey("SentToId")]
        public ApiUser SentTo { get; set; }
    }
}
