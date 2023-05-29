using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Helpers
{
    public class Meta<T>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }

        public int NextPage { get; set; }
        public int PreviousPage { get; set; }
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
        public Meta(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
