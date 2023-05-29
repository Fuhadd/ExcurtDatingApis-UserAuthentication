using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Helpers
{
    public class PaginationHelper
    {
        public static PageResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, int PageNumber, int PageSize, int totalCounts)
        {
            var responseMeta = new Meta<List<T>>(PageNumber, PageSize);
            var response = new PageResponse<List<T>>(pagedData, responseMeta);
            var totalPages = ((double)totalCounts / (double)PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.Meta.NextPage =
                PageNumber >= 1 && PageNumber < roundedTotalPages ?
                PageNumber + 1 : 0;

            response.Meta.PreviousPage =
                PageNumber - 1 >= 1 && PageNumber <= roundedTotalPages ?
                PageNumber - 1 : 0;

            response.Meta.FirstPage = 1;
            response.Meta.LastPage = roundedTotalPages;
            response.Meta.TotalPages = roundedTotalPages == 0 ? 1 : roundedTotalPages;
            response.Meta.TotalCount = totalCounts;
            return response;
        }
    }
}
