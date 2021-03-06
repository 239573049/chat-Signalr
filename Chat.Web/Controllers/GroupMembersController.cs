using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
﻿using Chat.Web.Code;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Model.GroupsVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IRedisUtil redisUtil;
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly IFriendsService friendsService;
        private readonly IGroupMembersService groupMembersService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="redisUtil"></param>
        /// <param name="chatHub"></param>
        /// <param name="friendsService"></param>
        /// <param name="principalAccessor"></param>
        /// <param name="groupMembersService"></param>
        public GroupMembersController(
            IMapper mapper,
            IRedisUtil redisUtil,
            IHubContext<ChatHub> chatHub,
            IFriendsService friendsService,
            IPrincipalAccessor principalAccessor,
            IGroupMembersService groupMembersService
            )
        {
            this.mapper = mapper;
            this.chatHub = chatHub;
            this.redisUtil = redisUtil;
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
            list.AddRange(data.Select(a => new GroupDataVM { Key = key++, ChatId = a.GroupDataId, Count = 0, Id = a.Id, Data = a.GroupData, IsGroup = true, SelfId = a.SelfId,Receiving=a.GroupData.Receiving }).ToList());
            list.AddRange(friends.Select(a => new GroupDataVM { Key = key++, ChatId = a.FriendId, Count = 0, Data = a.Friend, Id = a.Id, IsGroup = false, SelfId = a.SelfId,Receiving=a.Receiving }));
            return list;
        }
        /// <summary>
        /// 添加群成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddGroup(Guid groupId, List<Guid> ids)
        {
            var data= await groupMembersService.AddGroup(groupId, ids);
            var receiving = new List<string>();
            foreach (var d in ids) {
                receiving.AddRange(await redisUtil.SMembersAsync<string>("connection"+d));
            }
            receiving.ForEach(async a => await chatHub.Groups.AddToGroupAsync(a, data.Receiving));
            return true;
        }
    }
}
