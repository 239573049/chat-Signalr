using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat.Application.AppServices.UserService;
using Chat.Code.DbEnum;
using Chat.Code.Entities.User;
using Chat.Uitl.Util;
using Chat.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;
using Cx.NetCoreUtils.Extensions;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 登录接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly IRedisUtil redisUtil;
        private readonly IPrincipalAccessor principalAccessor;
        public LoginController(
            IMapper mapper,
            IRedisUtil redisUtil,
            IAccountService accountService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.redisUtil = redisUtil;
            this.accountService = accountService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Register(string accountNumber,string passWord)
        {
            var data =await accountService.GetAccount(accountNumber);
            if (data.PassWrod != passWord.MD5Encrypt()) return new ModelStateResult("账号或者密码错误");
            switch (data.Status)
            {
                case StatusEnum.Disabled:
                    return new ModelStateResult("账号已禁用请联系管理员");
                case StatusEnum.Freeze:
                    return new ModelStateResult($"账号已冻结至{(DateTime)data.Freezetime:yyyy-MM-dd HH:mm:ss}");
            }
            var token = StringUtil.GetString(50);
            await redisUtil.SetAsync(token, data,DateTime.Now.AddMinutes(30));
            HttpContext.Response.Cookies.Append("id",$"{data.Id}");
            return new OkObjectResult(new { token,user=data});
        }
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> RefreshToken()
        {
            var token = principalAccessor.GetToken();
            return new OkObjectResult(await redisUtil.SetDateAsync(token, DateTime.Now.AddMinutes(30)));
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Exit()
        {
            var token = principalAccessor.GetToken();
            return new OkObjectResult( await redisUtil.DeleteAsync(token));
        }
    }
}
