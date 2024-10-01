using AutoMapper;

using Messanger.Domain.Entities;
using Messanger.Application.DTOs;

namespace Messanger.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
        }
    }
}