using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private ApiUser _user;

        public Authentication(UserManager<ApiUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;

        }

        public async Task<bool> AuthenticateAsync(LoginUserDTO user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email);
            return (user!= null && await _userManager.CheckPasswordAsync(_user, user.Password));
        }

        public async Task<RefreshTokenResponse> CreateToken(ApiUser _user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(_user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = new RefreshToken()
            {
                JwtId = "The FREAKKK",
                IsUsed = false,
                IsRevoked = false,
                CreatedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6),
                Token = RandomString(25) + Guid.NewGuid(),
                ApiUserId = _user.Id,
            };
            await _unitOfWork.RefreshTokens.Create(refreshToken);
            await _unitOfWork.Save();

            return new RefreshTokenResponse()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var expiration = DateTime.Now.AddHours(24);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims:claims,
                signingCredentials:signingCredentials,
                expires:expiration);
            return token;
        }

        private async Task<List<Claim>> GetClaims(ApiUser _user)
        {
            var claim = new List<Claim>(new List<Claim>
            {
                new Claim(ClaimTypes.Actor, _user.FirstName),
                new Claim(ClaimTypes.Name, _user.Id.ToString()),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, "The FREAKKK"),
            }) ;
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            return claim;
        }

        private SigningCredentials GetSigningCredentials()
        {
            string key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials (secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
