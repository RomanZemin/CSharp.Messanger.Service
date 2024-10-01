using AutoMapper;

using Messanger.Domain.Entities;
using Messanger.Application.DTOs;

namespace Messanger.Application.Mappings
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDTO>()
                .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.Admin.Id));
        }
    }
}
