using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UserAuthenticationService.Core.Hubs;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Core.Services.Authentication;
using UserAuthenticationService.Core.Services.RefreshTokenServices;
using UserAuthenticationService.Core.Services.UserClaims;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IRefreshToken _refreshToken;
        private readonly IUnitOfWork _unitofWork;
        private readonly IUserClaims _userClaims;

        public ChatsController( IHubContext<ChatHub> hubContext,
                                UserManager<ApiUser> userManager,
                                IMapper mapper,
                                IAuthentication authentication,
                                IRefreshToken refreshToken,
                                IUnitOfWork unitofWork,
                                IUserClaims userClaims)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _mapper = mapper;
            _authentication = authentication;
            _refreshToken = refreshToken;
            _unitofWork = unitofWork;
            _userClaims = userClaims;
        }

        /// <summary>
        /// Create Message
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("create Message")]
        [Route("create-message")]
        [Authorize]

        public async Task<IActionResult> CreateMessage(Message message)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);

            message.ApiUserId = user.Id;
            await _unitofWork.Messages.Create(message);
            await _unitofWork.Save();
            return Ok();
        }

        /// <summary>
        /// Get All Messages
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get All Messages")]
        [Route("get-all-messages")]
        [Authorize]

        public async Task<IActionResult> GetAllMessage()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);

            var messages = await _unitofWork.Messages.GetAll();
            return Ok(messages);
        }

        /// <summary>
        /// Push Message
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("push Message")]
        [Route("push-message")]
        [Authorize]

        public async Task<IActionResult> PushMessage(Message message)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);

            message.ApiUserId = user.Id;
            await _unitofWork.Messages.Create(message);
            await _unitofWork.Save();
            _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok(message);
        }

    }
}
