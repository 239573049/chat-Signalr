using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
﻿using Chat.Web.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 好友列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class FriendsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly IFriendsService friendsService;
        public FriendsController(
            IMapper mapper,
            IFriendsService friendsService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.friendsService = friendsService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<FriendsDto>> GetFriendsDtos()
        {
            var userDto =await principalAccessor.GetUser<UserDto>();
            return await friendsService.GetFriendsDtos(userDto.Id);
        }
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteFriends(Guid friendId)
        {
            var userDto = await principalAccessor.GetUser<UserDto>();
            return await friendsService.DeleteFriends(userDto.Id, friendId);
        }
    }
}
