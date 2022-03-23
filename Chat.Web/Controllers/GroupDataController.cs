using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
using Cx.NetCoreUtils.Exceptions;
﻿using Chat.Web.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;
using Chat.Web.Code.Gadget;
using Microsoft.AspNetCore.SignalR;
using static Chat.Web.Code.Middleware.CurrentLimiting;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 群
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class GroupDataController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRedisUtil redisUtil;
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IGroupDataService groupDataService;
        private readonly IPrincipalAccessor principalAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="redisUtil"></param>
        /// <param name="chatHub"></param>
        /// <param name="groupDataService"></param>
        /// <param name="principalAccessor"></param>
        public GroupDataController(
            IMapper mapper,
            IRedisUtil redisUtil,
            IHubContext<ChatHub> chatHub,
            IGroupDataService groupDataService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.chatHub = chatHub;
            this.redisUtil = redisUtil;
            this.groupDataService = groupDataService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 创建群
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateGroupData(GroupDataDto group)
        {
            var user = await principalAccessor.GetUser<UserDto>();
            group = await groupDataService.CreateGroupData(group, user);
            var ids = await redisUtil.SMembersAsync<string>("connection" + user.Id);
            for (int i = 0; i < ids.Length; i++) {
                await chatHub.Groups.AddToGroupAsync(ids[i], group.Receiving);
            }
            return new OkObjectResult("创建成功");
        }
        /// <summary>
        /// 删除群
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteGroupData(Guid id)
        {
            var user =await principalAccessor.GetUser<UserDto>();
            return new OkObjectResult(await groupDataService.DeleteGroupData(id,user.Id));
        }
        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<GroupDataDto> GetGroup(Guid id) =>
            await groupDataService.GetGroup(id);
    }
}
