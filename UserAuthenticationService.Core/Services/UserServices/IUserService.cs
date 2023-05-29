using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models.Response;

namespace UserAuthenticationService.Core.Services.UserServices
{
    public interface IUserService
    {
        Task<ServiceResponse> RegisterEmail(RegisterEmailDTO userDTO);
        Task<ServiceResponse> UpdateMobileNumber(UpdateMobileNumberDTO userDTO);
        Task<ServiceResponse> UpdateName(UpdateNameDTO userDTO);
        Task<ServiceResponse> UpdateBirthDay(UpdateBirthDayDTO userDTO);
        Task<ServiceResponse> UpdateGender(UpdateGenderDTO userDTO);
        Task<ServiceResponse> UpdateInterest(UpdateInterestDTO userDTO);
        Task<ServiceResponse> Login(LoginUserDTO user);
        Task<ServiceResponse> GetUserInformation();
        Task<ServiceResponse> GetUserByEmail();
        Task<ServiceResponse> GetAllUser();
        Task<ServiceResponse> RefreshToken(TokenRequestDTO tokenRequest);
        Task<ServiceResponse> UploadUserImage(List<UploadUserImageDTO> uploadUserImageDTO);
        Task<ServiceResponse> DeleteUserImage(int ImageId);
        Task<ServiceResponse> UploadSingleUserImage(UploadUserImageDTO uploadUserImageDTO);
    }
}
