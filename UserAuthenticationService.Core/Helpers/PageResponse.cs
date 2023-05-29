using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.Models.Response;

namespace UserAuthenticationService.Core.Helpers
{
    public class PageResponse<T> : ApiResponse<T>
    {
        public Meta<T> Meta { get; set; }

        public PageResponse(T data, Meta<T> meta)
        {
            Meta = meta;
            this.data = data;
            this.message = null;
            this.errors = null;
            this.success = null;
        }
    }
}
