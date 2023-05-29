using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthenticationService.Core.DTOs;
using UserAuthenticationService.Core.Models.Response;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Core.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<ApiUser, ApiUserDTO>().ReverseMap();
            CreateMap<UserImage, UserImageDTO>().ReverseMap();
            CreateMap<UserImage, UserImageDTO>().ReverseMap();
            CreateMap<MatchedUserResponse, MatchedUserResponseDTO>();
            CreateMap<ApiUser, RegisterEmailDTO>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();
            CreateMap<UserImage, UploadUserImageDTO>().ReverseMap();
        }
    }
}
