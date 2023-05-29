using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Models.Response
{
    public class ServiceResponse
    {
        public bool success { get; set; }
        public object? data { get; set; }
        public string? message { get; set; }
        public List<string>? errors { get; set; }
    }
}
