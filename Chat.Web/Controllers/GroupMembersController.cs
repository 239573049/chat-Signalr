using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
﻿using Chat.Web.Code;
using Chat.Web.Code.Model.GroupsVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 群列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class GroupMembersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly IFriendsService friendsService;
        private readonly IGroupMembersService groupMembersService;
        public GroupMembersController(
            IMapper mapper,
            IFriendsService friendsService,
            IPrincipalAccessor principalAccessor,
            IGroupMembersService groupMembersService
            )
        {
            this.mapper = mapper;
            this.friendsService = friendsService;
            this.principalAccessor = principalAccessor;
            this.groupMembersService = groupMembersService;
        }
        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GroupDataVM>> GetGroupMembersList()
        {
            var user = await principalAccessor.GetUser<UserDto>();
            var data =await groupMembersService.GetGroupMembersList(user.Id);
            var friends = await friendsService.GetFriendsDtos(user.Id);
            var list = new List<GroupDataVM>();
            var key = 0;
            list.AddRange(data.Select(a => new GroupDataVM { Key = key++, ChatId = a.GroupDataId, Count = 0, Id = a.Id, Data = a.GroupData, IsGroup = true, SelfId = a.SelfId }).ToList());
            list.AddRange(friends.Select(a => new GroupDataVM { Key = key++, ChatId = a.FriendId, Count = 0, Data = a.Friend, Id = a.Id, IsGroup = false, SelfId = a.SelfId }));
            return list;
        }
    }
}
