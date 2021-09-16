using AutoMapper;
using Chat.Application.Dto.GroupsDto;
using Chat.Web.Code.Model.ChatVM;
using static Chat.Web.Code.EnumWeb.EnumWeb;

namespace Chat.Web.Code.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ChatMessageDto, MessageVM>()
                .ForMember(a=>a.Marking,dest=>dest.MapFrom(det=>(ChatMessageEnum)det.Marking));
            CreateMap<MessageVM, ChatMessageDto>()
                .ForMember(a => a.Marking, dest => dest.MapFrom(det => (sbyte)det.Marking));
        }
    }
}
