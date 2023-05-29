using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Core.Services.Authentication;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Services.RefreshTokenServices
{
    public class RefreshTokenServices : IRefreshToken
    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthentication _authentication;

        public RefreshTokenServices(TokenValidationParameters tokenValidationParams,
                            IUnitOfWork unitOfWork,
                            IMapper mapper,
                            UserManager<ApiUser> userManager,
                            IAuthentication authentication)
        {
            _tokenValidationParams = tokenValidationParams;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _authentication = authentication;

        }

        public async Task<RefreshTokenResponse> VerifyAndGenerateRefreshTokenAsync(TokenRequestDTO tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //VALIDATION 1: Validate JWT Token Format
                _tokenValidationParams.ValidateLifetime = false;
                var tokenInVerification = jwtTokenHandler.
                    ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;

                //VALIDATION 2: Validate Encryption Algorithm

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);

                    if (result == false)
                    {
                        return null;
                    }
                }

                //VALIDATION 3: Validate Expiry Date

                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = unixTimeStampToDateTime(utcExpiryDate);
                if (expiryDate > DateTime.Now)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Token Has Not Yet Expired"
                        }
                    };
                }

                //VALIDATION 4: Check if Refresh Token Exists in Database

                var storedToken = await _unitOfWork.RefreshTokens.Get(expression: x =>
                                        x.Token == tokenRequest.RefreshToken);
                if (storedToken == null)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Token Does Not Exist"
                        }
                    };
                }

                //VALIDATION 5: Check if Refresh Token Has Been Used
                if (storedToken.IsUsed)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Has Been Used"
                        }
                    };

                }

                //VALIDATION 6: Check if Refresh Token Has Been Revoked
                if (storedToken.IsRevoked)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Has Been Revoked"
                        }
                    };

                }

                //VALIDATION 7: Confirm Jti
                var jti = tokenInVerification.Claims.FirstOrDefault(x =>
                        x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Current Refresh Token Id Does Not Match"
                        }
                    };

                }

                //Update Current Token
                storedToken.IsUsed = true;
                storedToken = _mapper.Map<RefreshToken>(storedToken);

                _unitOfWork.RefreshTokens.Update(storedToken);
                await _unitOfWork.Save();

                //var apiUser = await _userManager.FindByIdAsync(storedToken.ApiUserId.ToString());
                var apiUser = await _unitOfWork.ApiUsers.Get(expression:x=>x.Id == storedToken.ApiUserId);
                return await _authentication.CreateToken(apiUser);


            }
            catch(Exception e)
            {
                return new RefreshTokenResponse()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            e.Message
                        }
                };

            }
        }

        private DateTime unixTimeStampToDateTime(long utcExpiryDate)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(utcExpiryDate).ToLocalTime();
            return dateTimeVal;
        }
    }
}
