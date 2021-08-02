using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 群
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupDataController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGroupDataService groupDataService;
        private readonly IPrincipalAccessor principalAccessor;
        public GroupDataController(
            IMapper mapper,
            IGroupDataService groupDataService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.groupDataService = groupDataService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 创建群
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> CreateGroupData(GroupDataDto group)
        {
            var user = await principalAccessor.GetUser<UserDto>();
            return await groupDataService.CreateGroupData(group, user);
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
