using AutoMapper;

using Messanger.Domain.Entities;
using Messanger.Application.DTOs;

namespace Messanger.Application.Mappings
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.Username))
                .ForMember(dest => dest.FromFullName, opt => opt.MapFrom(src => src.FromUser.FullName));
        }
    }
}
