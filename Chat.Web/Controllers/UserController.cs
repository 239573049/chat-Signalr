using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat.Application.AppServices.UserService;
using Chat.Code.DbEnum;
using Chat.Code.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Application.Dto;
using Chat.Web.Code.Model;
using Chat.Web.Code;
using Chat.Uitl.Util;
using Cx.NetCoreUtils.Exceptions;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;
using Cx.NetCoreUtils.Extensions;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 账号接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService UserService;
        private readonly IPrincipalAccessor principalAccessor;
        public UserController(
            IMapper mapper,
            IUserService UserService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.UserService = UserService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 新增账号
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> CreateUser(UserDto User)
        {
            return await UserService.CreateUser(User);
        }
        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="time">封至时间</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> FreezeUser(List<Guid> ids, DateTime time) =>
            await UserService.FreezeUser(ids, time);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PeddleDataResponse<IList<UserDto>>> GetUserList(string name,sbyte status=-1, int pageNo = 1, int pageSize = 20)
        {
            var userDto =await principalAccessor.GetUser<UserDto>();
            if (userDto.Power != PowerEnum.Manage) throw new BusinessLogicException("权限不足，请联系管理员");
            var data = await UserService.GetUserList(name, status, pageNo, pageSize);
            return new PeddleDataResponse<IList<UserDto>>(SerialNumber.GetList(data.Item1, pageNo, pageSize), data.Item2);
        }
        /// <summary>
        /// 获取用户账号
        /// </summary>
        /// <param name="UserNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserDto> GetUserName(string UserNumber) =>
            await UserService.GetUser(UserNumber);
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> UpdateUser(UserDto user) =>
            await UserService.UpdateUser(user);
        /// <summary>
        /// 管理员编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatesUsers(UserDto user)
        {
            var userDto = await principalAccessor.GetUser<UserDto>();
            if (userDto.Power != PowerEnum.Manage) throw new BusinessLogicException("权限不足，请联系管理员");
            if (user.PassWrod.Length < 5) return new ModelStateResult("密码长度不能小于五位");
            if (user.UserNumber.Length < 5) return new ModelStateResult("用户名长度不能小于五位");
            user.PassWrod = user.PassWrod.MD5Encrypt();
            return new OkObjectResult(await UserService.UpdatesUsers(user)?"编辑成功":"编辑失败");
        }
    }
}
