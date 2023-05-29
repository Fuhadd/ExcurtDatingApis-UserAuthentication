using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Helpers;
using UserAuthenticationService.Core.Services.MatchesServices;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesServices _matchesServices;

        public MatchesController(IMatchesServices matchesServices)
        {
            _matchesServices = matchesServices;

        }

        /// <summary>
        /// Get All Users For Matching
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("get-all-users-for-matching")]
        [Route("get-all-users-for-matching")]
        [Authorize]

        public async Task<IActionResult> GetUsersForMatching([FromQuery] PaginationFilter filter)
        {
            var ValidFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var userResponse = await _matchesServices.GetUsersForMatching(ValidFilter);
            int totalRecords = await _matchesServices.GetTotalUsers(filter);
            var PagedResponse = PaginationHelper.CreatePagedResponse<ApiUser>(userResponse, ValidFilter.PageNumber, ValidFilter.PageSize, totalRecords);
            PagedResponse.success = true;
            return Ok(PagedResponse);
        }

        /// <summary>
        /// Get All Users For Matching 2
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("get-all-users-for-matching2")]
        [Route("get-all-users-for-matching2")]
        [Authorize]

        public async Task<IActionResult> GetUsersForMatching2([FromQuery] PaginationFilter filter)
        {
            var ValidFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var userResponse = await _matchesServices.GetUsersForMatching2(ValidFilter);
            int totalRecords = await _matchesServices.GetTotalUsers(filter);
            var PagedResponse = PaginationHelper.CreatePagedResponse<ApiUser>(userResponse, ValidFilter.PageNumber, ValidFilter.PageSize, totalRecords);
            PagedResponse.success = true;
            return Ok(PagedResponse);
        }

        /// <summary>
        /// Like a User
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Like a User")]
        [Route("like-user")]
        [Authorize]
        public async Task<IActionResult> LikeUser(int matchedUserId, double? similarities)
        {
            var response = await _matchesServices.LikeUser(matchedUserId, similarities);

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// DisLike a User
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("DisLike a User")]
        [Route("dislike-user")]
        [Authorize]
        public async Task<IActionResult> DisLikeUser(int matchedUserId, double? similarities)
        {
            var response = await _matchesServices.DisLikeUser(matchedUserId, similarities);

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get All Liked Users By Id
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get All Liked Users By Id")]
        [Route("get-all-liked-users")]
        [Authorize]
        public async Task<IActionResult> GetAllLikedUsersById()
        {
            var response = await _matchesServices.GetAllLikedUsersById();

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get All Liked Users By Id No Chat
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get All Liked Users By Id No Chat")]
        [Route("get-all-liked-users-no-chat")]
        [Authorize]
        public async Task<IActionResult> GetAllLikedUsersByIdNoChat()
        {
            var response = await _matchesServices.GetAllLikedUsersByIdNoChat();

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
            
        }
        /// <summary>
        /// Get All Liked Users By Id With Chat
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get All Liked Users By Id With Chat")]
        [Route("get-all-liked-users-with-chat")]
        [Authorize]
        public async Task<IActionResult> GetAllLikedUsersByIdWithChat()
        {
            var response = await _matchesServices.GetAllLikedUsersByIdWithChat();

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
