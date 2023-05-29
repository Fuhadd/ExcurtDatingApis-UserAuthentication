using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Services.UserServices;

namespace UserAuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserIdentityController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserIdentityController(IUserService userService)
        {
            _userService = userService;

        }

        /// <summary>
        /// Create A New User
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPost, EndpointName("Create A New User")]
        [Route("create-user")]
        public async Task<IActionResult> Register([FromBody] RegisterEmailDTO model)
        {
            var response = await _userService.RegisterEmail(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPost, EndpointName("Login")]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO model)
        {
            var response = await _userService.Login(model);

            if (response.success)
            {

                return Ok(response);
            }

            return Unauthorized(response);
        }

        [HttpGet, EndpointName("Get User By Email")]
        [Route("get-user-by-email")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail()
        {
            var response = await _userService.GetUserByEmail();

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get User's Info
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get User Information")]
        [Route("get-user-information")]
        [Authorize]
        public async Task<IActionResult> GetUserInformation()
        {
            var response = await _userService.GetUserInformation();

            if (response.success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet, EndpointName("Get All Users")]
        [Route("get-all-user")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            var response = await _userService.GetAllUser();

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost, EndpointName("Refresh Token")]
        [Route("refresh-token")]
        public async Task<IActionResult> GetAllUser([FromBody] TokenRequestDTO model)
        {
            var response = await _userService.RefreshToken(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload Mobile Number
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload Mobile Number")]
        [Route("upload-mobile-number")]
        [Authorize]
        public async Task<IActionResult> UpdateMobileNumber([FromBody] UpdateMobileNumberDTO model)
        {
            var response = await _userService.UpdateMobileNumber(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload User Name
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload User Name")]
        [Route("upload_user-name")]
        [Authorize]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameDTO model)
        {
            var response = await _userService.UpdateName(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload Single User Image
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload Single User Image")]
        [Route("upload-single-user-image")]
        [Authorize]
        public async Task<IActionResult> UploadSingleUserImage([FromBody] UploadUserImageDTO model)
        {
            var response = await _userService.UploadSingleUserImage(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Delete User Image
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpDelete, EndpointName("Delete User Image")]
        [Route("delete-user-image/{imageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserImage(int imageId)
        {
            var response = await _userService.DeleteUserImage(imageId);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload User Birthday
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload User Birthday")]
        [Route("upload_user-birthday")]
        [Authorize]
        public async Task<IActionResult> UpdateBirthDay([FromBody] UpdateBirthDayDTO model)
        {
            var response = await _userService.UpdateBirthDay(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload User Gender
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload User Gender")]
        [Route("upload_user-gender")]
        [Authorize]
        public async Task<IActionResult> UpdateGender([FromBody] UpdateGenderDTO model)
        {
            var response = await _userService.UpdateGender(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload User Image
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload User Image")]
        [Route("upload_user-image")]
        [Authorize]
        public async Task<IActionResult> UploadUserImage([FromBody] List<UploadUserImageDTO> model)
        {
            var response = await _userService.UploadUserImage(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Upload User Interests
        /// </summary>
        /// <param model=""></param>
        /// <returns></returns>
        [HttpPatch, EndpointName("Upload User Interests")]
        [Route("upload_user-interests")]
        [Authorize]
        public async Task<IActionResult> UpdateInterest([FromBody] UpdateInterestDTO model)
        {
            var response = await _userService.UpdateInterest(model);

            if (response.success)
            {

                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
