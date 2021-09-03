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
using Org.BouncyCastle.Crypto;

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
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            if (user.PassWord.Length < 5) return new ModelStateResult("密码长度不能少于五位");
            if (user.PassWord!=user.PassWords) return new ModelStateResult("俩次输入的密码不一致");
            if (user.UserNumber.Length < 5) return new ModelStateResult("密码长度不能少于五位");
            if (string.IsNullOrEmpty(user.HeadPortrait)) return new ModelStateResult("头像不能为空");
            if (await UserService.IsExist(user.UserNumber)) return new ModelStateResult("账号已经存在请重新编辑");
            return new OkObjectResult(await UserService.CreateUser(user));
        }
        /// <summary>
        /// 是否存在用户名
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> IsExist(string userNumber) =>
            await UserService.IsExist(userNumber);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteUser(Guid id)
        {
            var userDto =await principalAccessor.GetUser<UserDto>();
            if (userDto.Power != PowerEnum.Manage) throw new BusinessLogicException("权限不足，请联系管理员");
            if (userDto.Id == id) throw new BusinessLogicException("无法删除本账号");
            return await UserService.DeleteUser(id);
        }
        /// <summary>
        /// 批量删除账号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteUserList(List<Guid> ids)
        {
            var userDto = await principalAccessor.GetUser<UserDto>();
            if (userDto.Power != PowerEnum.Manage) throw new BusinessLogicException("权限不足，请联系管理员");
            var isUser = ids.FirstOrDefault(a=>a==userDto.Id);
            if (isUser!=Guid.Empty) 
            {
                ids.Remove(isUser);
            }
            return await UserService.DeleteUserList(ids);
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
        /// <param name="userNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserDto> GetUserName(string userNumber)
        {
            var data= await UserService.GetUser(userNumber);
            data.PassWord = null;
            return data;
        }
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
            if (user.PassWord.Length < 5) return new ModelStateResult("密码长度不能小于五位");
            if (user.UserNumber.Length < 5) return new ModelStateResult("用户名长度不能小于五位");
            user.PassWord = user.PassWord.MD5Encrypt();
            return new OkObjectResult(await UserService.UpdatesUsers(user)?"编辑成功":"编辑失败");
        }

    }
}
