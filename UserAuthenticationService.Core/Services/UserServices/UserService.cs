using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models.Response;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Core.Services.Authentication;
using UserAuthenticationService.Core.Services.RefreshTokenServices;
using UserAuthenticationService.Core.Services.UserClaims;
using UserAuthenticationService.Data.Data;
using UserAuthenticationService.Data.Utilities;
using static UserAuthenticationService.Data.Utilities.Enums;

namespace UserAuthenticationService.Core.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IRefreshToken _refreshToken;
        private readonly IUnitOfWork _unitofWork;
        private readonly IUserClaims _userClaims;

        public UserService(UserManager<ApiUser> userManager,
                                IMapper mapper,
                                IAuthentication authentication,
                                IRefreshToken refreshToken,
                                IUnitOfWork unitofWork,
                                IUserClaims userClaims)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authentication = authentication;
            _refreshToken = refreshToken;
            _unitofWork = unitofWork;
            _userClaims = userClaims;
        }

        public async Task<ServiceResponse> GetAllUser()
        {
            var result = _userManager.Users.ToList();
            var user = _mapper.Map<IList<ApiUserDTO>>(result);

            return new ServiceResponse
            {
                success = true,
                data = user,
                message = $"Users fetched Successfully",
            };
        }

        public async Task<ServiceResponse> GetUserInformation()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser result = await _unitofWork.ApiUsers.Get(expression:x => (x.Email == email), includes: new List<string> { "UserImages" });
            ApiUserDTO user = _mapper.Map<ApiUserDTO>(result);
            return new ServiceResponse
            {
                success = true,
                data = user,
                message = $"User fetched Successfully",
            };
        }
        public async Task<ServiceResponse> GetUserByEmail()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser result = await _userManager.FindByEmailAsync(email);
            ApiUserDTO user = _mapper.Map<ApiUserDTO>(result);
            return new ServiceResponse
            {
                success = true,
                data = user,
                message = $"User fetched Successfully",
            };
        }

        public async Task<ServiceResponse> Login(LoginUserDTO user)
        {
            if (!await _authentication.AuthenticateAsync(user))
            {
                return new ServiceResponse
                {
                    success = false,
                    message = "unauthenticated",
                    errors = new List<string>
                    {
                        "Email or Password Incorrect, please try again"
                    }
                };
            }
            ApiUser _user = await _userManager.FindByEmailAsync(user.Email);
            var jwtToken = await _authentication.CreateToken(_user);
            return new ServiceResponse
            {
                success = true,
                data = jwtToken,
                message = $"User logged in Successfully",
            };

        }

        public async Task<ServiceResponse> RefreshToken(TokenRequestDTO tokenRequest)
        {
            var token = await _refreshToken.VerifyAndGenerateRefreshTokenAsync(tokenRequest);
            return new ServiceResponse
            {
                success = true,
                data = token,
                message = $"Refresh Token Fetched Successfully",
            };
        }


        public async Task<ServiceResponse> RegisterEmail(RegisterEmailDTO userDTO)
        {
            var user = _mapper.Map<ApiUser>(userDTO);
            //Check If Email Exists in Db first
            var user_exist = await _userManager.FindByEmailAsync(user.Email);

            if (user_exist != null)
            {
                return new ServiceResponse
                {
                    success = false,
                    message = $"User with email {user.Email} already exists",
                    errors = new List<string>
                    {
                        "Email already exist"
                    }
                };
            }

            user.UserName = user.Email;
            user.Hash = userDTO.Password;
            user.OnboardingStep = (int)OnboardingStep.UpdateMobileNumber;
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                return new ServiceResponse
                {
                    success = false,
                    message = $"Failed to register user ",
                    errors = result.Errors.Select(error => error.Description).ToList()
                };
            };
            var jwtToken = await _authentication.CreateToken(user);
            return new ServiceResponse
            {
                success = true,
                data = new RegisterEmailResponse
                {
                   JwtToken = jwtToken,
                   Email = user.Email,
                   OnboardingStep = user.OnboardingStep,
                },
                message = $"User created Successfully",
            };
        }

        public async Task<ServiceResponse> UpdateBirthDay(UpdateBirthDayDTO userDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdateBirthday)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to update Date of birth"
                        },
                        message = $"Date of birth Update Failed",
                    };

                }
                if ( (DateTime.Now.Year - userDTO.DateOfBirth.Year) < 18)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Chill, Come back when you are above 18 years of Age"
                        },
                        message = $"Date of Birth Update Failed",
                    };

                }
                user.DateOfBirth = userDTO.DateOfBirth;
                user.Age = userDTO.Age;
                user.OnboardingStep = (int)OnboardingStep.UpdateGender;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"Date of Birth Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"Date of Birth Update Failed",
            };
        }

        public async Task<ServiceResponse> UpdateGender(UpdateGenderDTO userDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdateGender)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to update gender"
                        },
                        message = $"Gender Update Failed",
                    };

                }
                user.Gender = userDTO.Gender;
                user.OnboardingStep = (int)OnboardingStep.UpdatePhotos;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"Date of Birth Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"Date of Birth Update Failed",
            };
        }

        //Use String seperated by iphen - for interests e.g "sport-driving-gaming"
        public async Task<ServiceResponse> UpdateInterest(UpdateInterestDTO userDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdateInterests)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to update interests"
                        },
                        message = $"User Interests Update Failed",
                    };

                }
                user.Interests = userDTO.Interests;
                user.OnboardingStep = (int)OnboardingStep.Done;
                user.Likeability = 0;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"User Interests Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"User Interests Update Failed",
            };
        }

        public async Task<ServiceResponse> UpdateMobileNumber(UpdateMobileNumberDTO userDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            if (user != null) 
            {
                if (!ValidationFunctions.IsValidNigerianPhoneNumber(userDTO.MobileNumber))
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Please Input a Valid Phone Number"
                        },
                        message = $"Mobile Number Update Failed",
                    };

                }
                var duplicatePhone = await _unitofWork.ApiUsers.Get(expression: o => (o.MobileNumber == userDTO.MobileNumber));
                if (duplicatePhone != null)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Phone number already exist"
                        },
                        message = $"Mobile Number Update Failed",
                    };

                }
                if (user.OnboardingStep < (int)OnboardingStep.UpdateMobileNumber)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to update mobile number"
                        },
                        message = $"Mobile Number Update Failed",
                    };

                }
                if (userDTO.Otp == null)
                {
                    await SendOTP(userDTO.MobileNumberDialCode, userDTO.MobileNumber);
                    return new ServiceResponse
                    {
                        success = true,
                        message = $"Otp sent to ${userDTO.MobileNumber} Successfully",
                    };
                }
                if (VerifyMobileNumber(userDTO.MobileNumber, userDTO.Otp))
                {
                    user.MobileNumber = userDTO.MobileNumber;
                    user.MobileNumberDialCode = userDTO.MobileNumberDialCode;
                    user.OnboardingStep = (int)OnboardingStep.UpdateName;
                    _unitofWork.ApiUsers.Update(user);
                    await _unitofWork.Save();
                    return new ServiceResponse
                    {
                        success = true,
                        data = user,
                        message = $"Mobile Number Uploaded Successfully",
                    };
                } else
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Incorrect Otp. Please try again"
                        },
                        message = $"Mobile Number Update Failed",
                    };
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"Mobile Number Update Failed",
            };
        }

        private bool VerifyMobileNumber(string? mobileNumber, string otp)
        {
            return (otp == "123456");
        }

        private Task SendOTP(string? mobileNumberDialCode, string? mobileNumber)
        {
            return Task.Delay(2000);
        }

        public async Task<ServiceResponse> UpdateName(UpdateNameDTO userDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdateName)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to update user name"
                        },
                        message = $"User Name Update Failed",
                    };

                }
                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.OnboardingStep = (int)OnboardingStep.UpdateBirthday;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"User Name Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"User Name Update Failed",
            };
        }

        public async Task<ServiceResponse> UploadUserImage(List<UploadUserImageDTO> uploadUserImageDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);
            
            
            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdatePhotos)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to upload user images"
                        },
                        message = $"User Image Upload Failed",
                    };

                }
                var oldUserImages = _unitofWork.UserImages.GetAll(expression: o => (o.ApiUserId == user.Id));
                foreach (UploadUserImageDTO image in uploadUserImageDTO)
                {
                    UserImage userImage = _mapper.Map<UserImage>(image);
                    userImage.ApiUserId = user.Id;
                    await _unitofWork.UserImages.Create(userImage);
                    await _unitofWork.Save();
                };
                if (user.OnboardingStep == (int)OnboardingStep.UpdatePhotos)
                    user.OnboardingStep = (int)OnboardingStep.UpdateInterests;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"User Images Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"User Image Update Failed",
            };
           
        }

        public async Task<ServiceResponse> UploadSingleUserImage(UploadUserImageDTO uploadUserImageDTO)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);


            if (user != null)
            {
                if (user.OnboardingStep < (int)OnboardingStep.UpdatePhotos)
                {
                    return new ServiceResponse
                    {
                        success = false,
                        errors = new List<string>
                        {
                            "Complete necessary requirements to upload user images"
                        },
                        message = $"User Image Upload Failed",
                    };

                }
                /*var oldUserImages = await _unitofWork.UserImages.GetAll(expression: o => (o.ApiUserId == user.Id));
                */
                UserImage userImage = _mapper.Map<UserImage>(uploadUserImageDTO);
                userImage.ApiUserId = user.Id;
                await _unitofWork.UserImages.Create(userImage);
                await _unitofWork.Save();
                  if (user.OnboardingStep == (int)OnboardingStep.UpdatePhotos)
                    user.OnboardingStep = (int)OnboardingStep.UpdateInterests;
                _unitofWork.ApiUsers.Update(user);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"User Images Updated Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"User Image Update Failed",
            };

        }

        public async Task<ServiceResponse> DeleteUserImage(int ImageId)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser user = await _userManager.FindByEmailAsync(email);


            if (user != null)
            {
                
                await _unitofWork.UserImages.Delete(ImageId);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = user,
                    message = $"User Images Deleted Successfully",
                };

            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "User does not exist"
                    },
                message = $"User Image Delete Failed",
            };

        }

    }

}



