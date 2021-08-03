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
        private readonly IGroupMembersService groupMembersService;
        public GroupMembersController(
            IMapper mapper,
            IPrincipalAccessor principalAccessor,
            IGroupMembersService groupMembersService
            )
        {
            this.mapper = mapper;
            this.principalAccessor = principalAccessor;
            this.groupMembersService = groupMembersService;
        }
        /// <summary>
        /// 获取群列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<GroupMembersDto>> GetGroupMembersList()
        {

            var user = await principalAccessor.GetUser<UserDto>();
            var data =await groupMembersService.GetGroupMembersList(user.Id);
            return data;
        }
    }
}
