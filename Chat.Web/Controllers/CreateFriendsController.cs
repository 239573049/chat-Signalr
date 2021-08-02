using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Code.DbEnum;
﻿using Chat.Web.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Web.Code.Model;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
using Chat.Application.Dto;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 添加申请好友
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class CreateFriendsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly ICreateFriendsService createFriendsService;
        public CreateFriendsController(
            IMapper mapper,
            IPrincipalAccessor principalAccessor,
            ICreateFriendsService createFriendsService
            )
        {
            this.mapper = mapper;
            this.principalAccessor = principalAccessor;
            this.createFriendsService = createFriendsService;
        }
        /// <summary>
        /// 通过或拒绝好友申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> ChangeCreateFriends(Guid id,CreateFriendsEnum create)
        {
            return await createFriendsService.ChangeCreateFriends(id, create);
        }
        /// <summary>
        /// 获取好友申请列表信息
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PeddleDataResponse<List<CreateFriendsDto>>> GetCreateFriendsDtos(int pageNo = 1, int pageSize = 20)
        {
            var userDto =await principalAccessor.GetUser<UserDto>();
            var data =await createFriendsService.GetCreateFriendsDtos(userDto.Id, pageNo, pageSize);
            return new PeddleDataResponse<List<CreateFriendsDto>>(data.Item1, data.Item2);
        }
    }
}
