using AutoMapper;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Code.DbEnum;
using Chat.Code.Entities.Groups;
using Chat.Code.Entities.User;
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
            CreateMap<Account, AccountDto>()
                .ForMember(dest=>dest.StatusName,det=>det.MapFrom(a=> EnumExtensionUtil.GetEnumStringVal(a.Status)));
            CreateMap<GroupData, GroupDataDto>();
            CreateMap<GroupMembers, GroupMembersDto>();
            CreateMap<Friends, FriendsDto>();
            CreateMap<CreateFriends, CreateFriendsDto>();
            #endregion domain to dto

            #region dto to domain
            CreateMap<AccountDto, Account>()
                .ForMember(dest => dest.Status, det => det.MapFrom(a => EnumExtensionUtil.GetEnumVal<StatusEnum>(a.StatusName)));
            CreateMap<GroupDataDto, GroupData>();
            CreateMap<GroupMembersDto, GroupMembers>();
            CreateMap<FriendsDto, Friends>();
            CreateMap<CreateFriendsDto, CreateFriends>();
            #endregion dto to domain



        }
    }
}