using AutoMapper;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Code.DbEnum;
using Chat.Code.Entities.Groups;
using Chat.Code.Entities.Users;
using Chat.Uitl.Util;
using System;
using System.Net.Mail;

namespace Merchants.Ams.Application
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            #region domain to dto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.StatusCode, det => det.MapFrom(a => EnumExtensionUtil.GetEnumStringVal(a.Status)))
                .ForMember(dest => dest.UseStateCode, det => det.MapFrom(a => EnumExtensionUtil.GetEnumStringVal(a.UseState)));
            CreateMap<GroupData, GroupDataDto>();
            CreateMap<GroupMembers, GroupMembersDto>();
            CreateMap<Friends, FriendsDto>();
            CreateMap<CreateFriends, CreateFriendsDto>();
            #endregion domain to dto

            #region dto to domain
            CreateMap<UserDto, User>();
            CreateMap<GroupDataDto, GroupData>();
            CreateMap<GroupMembersDto, GroupMembers>();
            CreateMap<FriendsDto, Friends>();
            CreateMap<CreateFriendsDto, CreateFriends>();
            #endregion dto to domain



        }
    }
}