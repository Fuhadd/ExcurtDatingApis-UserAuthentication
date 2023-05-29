using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Helpers;
using UserAuthenticationService.Core.Models.Response;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Core.Services.Authentication;
using UserAuthenticationService.Core.Services.UserClaims;
using UserAuthenticationService.Data.Data;
using UserAuthenticationService.Data.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static UserAuthenticationService.Data.Utilities.Enums;

namespace UserAuthenticationService.Core.Services.MatchesServices
{
    public class MatchesServices : IMatchesServices
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IUnitOfWork _unitofWork;
        private readonly IUserClaims _userClaims;
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _config;

        public MatchesServices(UserManager<ApiUser> userManager,
                                IMapper mapper,
                                IAuthentication authentication,
                                IUnitOfWork unitofWork,
                                IUserClaims userClaims, DatabaseContext dbContext, IConfiguration config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authentication = authentication;
            _unitofWork = unitofWork;
            _userClaims = userClaims;
            _dbContext = dbContext;
            _config = config;
        }
        /*public async Task<List<MatchedUserResponseDTO>> GetUsersForMatching(PaginationFilter filter)
          {
              string email = _userClaims.GetMyEmail();
              ApiUser currentUser = await _userManager.FindByEmailAsync(email);
              var DataLength = (filter.PageNumber - 1) * filter.PageSize;
              //working

              var users = _dbContext.Users
                          .Where(user => user != currentUser
                                         && user.Gender != currentUser.Gender
                                         && user.OnboardingStep == (int)OnboardingStep.Done);

              if (!users.Any()) 
              {
                  return new List<MatchedUserResponseDTO>();
              }

              var result = users
                  .Select(user => new {
                      User = user,
                      CurrentUserInterests = new List<String> { "fight", "talk" },// ValidationFunctions.SplitByDash(currentUser.Interests),
                      MatchedUserInterests = new List<String> { "fight", "talk" }, //ValidationFunctions.SplitByDash(user.Interests),
                      AgeDifference = Math.Abs(currentUser.Age.GetValueOrDefault() - user.Age.GetValueOrDefault())
                  })
                  .Select(x => new {
                      x.User,
                      SharedInterests = x.CurrentUserInterests.Intersect(x.MatchedUserInterests),
                      TotalInterests = x.CurrentUserInterests.Union(x.MatchedUserInterests),
                      AgePoints = x.AgeDifference <= 18 ? Math.Max(0, 5 - (x.AgeDifference / 3) - (x.AgeDifference % 3 == 0 ? 1 : 0)) : 0
                  })
                  .Select(x => new {
                      x.User,
                      Similarity = (double)(x.SharedInterests.Count() + x.AgePoints) / (double)(x.TotalInterests.Count() + 1)
                  })
                  .OrderByDescending(x => x.Similarity)
                  .Skip(DataLength)
                  .Take(filter.PageSize)
                  .Select(x => new MatchedUserResponse
                  {
                      User = x.User,
                      Similarity = x.Similarity
                  })
                  .ToList();

              var data = _mapper.Map<List<MatchedUserResponseDTO>>(result);


              return data;
          }
        */
        /* public async Task<object> GetUsersForMatching(PaginationFilter filter)
         {
             string email = _userClaims.GetMyEmail();
             ApiUser currentUser = await _userManager.FindByEmailAsync(email);
             var DataLength = (filter.PageNumber - 1) * filter.PageSize;
             //working
             var currentUserInterests = currentUser.Interests.Split('-');
             var users = _dbContext.Users
                         .Where(user => user != currentUser
                                        && user.Gender != currentUser.Gender
                                        && user.OnboardingStep == (int)OnboardingStep.Done).Select(user => new
                                        {
                                            User = user,
                                            MatchScore = user.Interests == null ? 0 : user.Interests.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Intersect(currentUserInterests).Count() / (double)currentUserInterests.Length
                                        }) ;
             return (users);

            /* if (!users.Any())
             {
                 return new List<MatchedUserResponseDTO>();
             }

             var result = users
                 .Select(user => new {
                     User = user,
                     CurrentUserInterests = ValidationFunctions.SplitByDash(currentUser.Interests),
                     MatchedUserInterests = ValidationFunctions.SplitByDash(user.Interests),
                     AgeDifference = Math.Abs(currentUser.Age.GetValueOrDefault() - user.Age.GetValueOrDefault())
                 })
                 .Select(x => new {
                     x.User,
                     SharedInterests = x.CurrentUserInterests.Intersect(x.MatchedUserInterests),
                     TotalInterests = x.CurrentUserInterests.Union(x.MatchedUserInterests),
                     AgePoints = x.AgeDifference <= 18 ? Math.Max(0, 5 - (x.AgeDifference / 3) - (x.AgeDifference % 3 == 0 ? 1 : 0)) : 0
                 })
                 .Select(x => new {
                     x.User,
                     Similarity = (double)(x.SharedInterests.Count() + x.AgePoints) / (double)(x.TotalInterests.Count() + 1)
                 })
                 .OrderByDescending(x => x.Similarity)
                 .Skip(DataLength)
                 .Take(filter.PageSize)
                 .Select(x => new MatchedUserResponse
                 {
                     User = x.User,
                     Similarity = x.Similarity
                 })
                 .ToList();

             var data = _mapper.Map<List<MatchedUserResponseDTO>>(result);


             return data; 
         }*/

        public async Task<List<ApiUser>> GetUsersForMatching2(PaginationFilter filter)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            using var connection = new SqlConnection(_config.GetConnectionString("sqlConnection"));
            var DataLength = (filter.PageNumber - 1) * filter.PageSize;
            var heroes = await connection.QueryAsync<ApiUser, UserImage, ApiUser>
                ("SELECT * , (SELECT COUNT(*)   FROM STRING_SPLIT(Interests, '-') AS UserInterests    " +
                "JOIN STRING_SPLIT(@currentUserInterests, '-') AS CurrentUserInterests ON UserInterests.value = " +
                "CurrentUserInterests.value) AS Similarity FROM [escurt].[dbo].[AspNetUsers] u  " +
                "LEFT JOIN [escurt].[dbo].[UserImages] ui ON u.Id = ui.ApiUserId WHERE u.Id != @currentUserId " +
                "AND u.Gender != @currentUserGender AND u.OnboardingStep == 7 ORDER BY Similarity DESC " +
                "OFFSET @dataLength ROWS   FETCH NEXT @pageSize ROWS ONLY;",
                (apiUser, UserImage) =>
                {
                    apiUser.UserImages = _dbContext.UserImages.Where(x=>x.ApiUserId == apiUser.Id).ToList();
                     return apiUser;
                }
               ,
               new { currentUserInterests = currentUser.Interests, currentUserId = currentUser.Id, currentUserGender = currentUser.Gender, dataLength = DataLength, pageSize = filter.PageSize},
            splitOn: "Id"
                );
            return heroes.DistinctBy(u => u.Id).ToList();

        }

        public async Task<List<ApiUser>> GetUsersForMatching(PaginationFilter filter)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            var DataLength = (filter.PageNumber - 1) * filter.PageSize;
            var data = _dbContext.Users.Where(x => x.Gender != currentUser.Gender)
            .Where(x=> x.OnboardingStep ==7)
            .Where(x=> x.Id != currentUser.Id)
            .Where(x => !_dbContext.UserMatches.Any(m => m.SentToId == x.Id && m.SentById == currentUser.Id))
            .Include(x => x.UserImages)
            .OrderByDescending(x => x.Likeability)
            .Skip(DataLength)
            .Take(filter.PageSize)
            .ToList();
            return data;

        }


        public async Task<int> GetTotalUsers(PaginationFilter filter)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            
            int result = _dbContext.Users.Where(user => user != currentUser && user.Gender != currentUser.Gender && user.OnboardingStep ==7)
                .Where(x => !_dbContext.UserMatches.Any(m => m.SentToId == x.Id && m.SentById == currentUser.Id))
                .Count();;
            
            return result;
  
        }

        public async Task<ServiceResponse> LikeUser(int matchedUserId, double? similarities)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            ApiUser matchedUser = await _unitofWork.ApiUsers.Get(expression: x => x.Id == matchedUserId);
            if (matchedUser != null)
            {
                matchedUser.Likeability = matchedUser.Likeability + 1;
                _unitofWork.ApiUsers.Update(matchedUser);
                await _unitofWork.Save();
                var mutualMatch = _dbContext.UserMatches
                       .Where(x => x.SentToId == currentUser.Id)
                       .FirstOrDefault();
                if (mutualMatch != null) 
                {
                    mutualMatch.IsAccepted = true;
                    return new ServiceResponse
                    {
                        success = true,
                        data = mutualMatch,
                        message = $"It is a match",
                    };

                }
                UserMatch data = new UserMatch
                {
                    IsAccepted = false,
                    IsRejected = false,
                    IsLike = true,
                    IsDislike = false,
                    SentById = currentUser.Id,
                    SentToId = matchedUser.Id,
                    CreatedAt = DateTime.Now,
                    HasChatted = false,
                    similarities = similarities ?? null
                };
                _unitofWork.UserMatches.Create(data);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = data,
                    message = $"User with Id {currentUser.Id} has Matched Successfully with User with id {matchedUser.Id}",
                };
            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "Matched User does not exist"
                    },
                message = $"Match User Failed",
            };

        }
        public async Task<ServiceResponse> DisLikeUser(int matchedUserId, double? similarities)
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            ApiUser matchedUser = await _unitofWork.ApiUsers.Get(expression: x => x.Id == matchedUserId);
            if (matchedUser != null)
            {
                matchedUser.Likeability = matchedUser.Likeability - 1;
                _unitofWork.ApiUsers.Update(matchedUser);
                await _unitofWork.Save();
                UserMatch data = new UserMatch
                {
                    IsAccepted = false,
                    IsRejected = false,
                    IsDislike = true,
                    IsLike =false,
                    SentById = currentUser.Id,
                    SentToId = matchedUser.Id,
                    CreatedAt = DateTime.Now,
                    HasChatted = false,
                    similarities = similarities ?? null
                };
                _unitofWork.UserMatches.Create(data);
                await _unitofWork.Save();
                return new ServiceResponse
                {
                    success = true,
                    data = data,
                    message = $"User with Id {currentUser.Id} has Matched Successfully with User with id {matchedUser.Id}",
                };
            }
            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "Matched User does not exist"
                    },
                message = $"Match User Failed",
            };

        }
        public async Task<ServiceResponse> GetAllLikedUsersById()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            var matchedUser =  _dbContext.UserMatches
                        .Where(x => x.SentById == currentUser.Id || x.SentToId == currentUser.Id)
                        .Where(x => x.IsAccepted == true)
                        .Include(x => x.SentTo)
                        .Include(x => x.SentBy)
                        .ToList();
            return new ServiceResponse
            {
                success = true,
                data = matchedUser,
                message = $"All Liked users by {currentUser.Id} has been fetched successfully",
            };

            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "Matched User does not exist"
                    },
                message = $"Fetching all liked users failed",
            };

        }

        public async Task<ServiceResponse> GetAllLikedUsersByIdWithChat()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            var matchedUser = _dbContext.UserMatches
                        .Where(x => x.SentById == currentUser.Id || x.SentToId == currentUser.Id)
                        .Where(x => x.IsAccepted == true)
                        .Where(x => x.HasChatted == true)
                        .Include(x => x.SentTo)
                        .Include(x => x.SentBy)
                        .ToList();
            return new ServiceResponse
            {
                success = true,
                data = matchedUser,
                message = $"All Liked users by {currentUser.Id} has been fetched successfully",
            };

            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "Matched User does not exist"
                    },
                message = $"Fetching all liked users failed",
            };

        }

        public async Task<ServiceResponse> GetAllLikedUsersByIdNoChat()
        {
            string email = _userClaims.GetMyEmail();
            ApiUser currentUser = await _userManager.FindByEmailAsync(email);
            var matchedUser = _dbContext.UserMatches
                        .Where(x => x.SentById == currentUser.Id || x.SentToId == currentUser.Id)
                        .Where(x => x.IsAccepted == true)
                        .Where(x => x.HasChatted == false)
                        .Include(x => x.SentTo)
                        .Include(x => x.SentBy)
                        .ToList();
            return new ServiceResponse
            {
                success = true,
                data = matchedUser,
                message = $"All Liked users by {currentUser.Id} has been fetched successfully",
            };

            return new ServiceResponse
            {
                success = false,
                errors = new List<string>
                    {
                        "Matched User does not exist"
                    },
                message = $"Fetching all liked users failed",
            };

        }

    }
}
