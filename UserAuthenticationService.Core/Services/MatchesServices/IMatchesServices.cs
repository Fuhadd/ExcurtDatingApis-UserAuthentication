using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Helpers;
using UserAuthenticationService.Core.Models.Response;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Services.MatchesServices
{
    public interface IMatchesServices
    {
        Task<List<ApiUser>> GetUsersForMatching(PaginationFilter filter);
        Task<List<ApiUser>> GetUsersForMatching2(PaginationFilter filter);
        Task<int> GetTotalUsers(PaginationFilter filter);
        Task<ServiceResponse> LikeUser(int matchedUserId, double? similarities);
        Task<ServiceResponse> DisLikeUser(int matchedUserId, double? similarities);
        Task<ServiceResponse> GetAllLikedUsersById();
        Task<ServiceResponse> GetAllLikedUsersByIdWithChat();
        Task<ServiceResponse> GetAllLikedUsersByIdNoChat();

    }
}
