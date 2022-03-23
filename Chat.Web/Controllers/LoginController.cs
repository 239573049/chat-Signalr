using AutoMapper;

using Chat.Application.AppServices.UserService;
using Chat.Code.DbEnum;
using Chat.Uitl.Util;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

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
        private readonly IUserService UserService;
        private readonly IRedisUtil redisUtil;
        private readonly IPrincipalAccessor principalAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="redisUtil"></param>
        /// <param name="UserService"></param>
        /// <param name="principalAccessor"></param>
        public LoginController(
            IMapper mapper,
            IRedisUtil redisUtil,
            IUserService UserService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.mapper = mapper;
            this.redisUtil = redisUtil;
            this.UserService = UserService;
            this.principalAccessor = principalAccessor;
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Register(string UserNumber,string passWord)
        {
            var data =await UserService.GetUser(UserNumber);
            if (data.PassWord != passWord) return new ModelStateResult("账号或者密码错误");
            switch (data.Status)
            {
                case StatusEnum.Disabled:
                    return new ModelStateResult("账号已禁用请联系管理员");
                case StatusEnum.Freeze:
                    return new ModelStateResult($"账号已冻结至{(DateTime)data.Freezetime:yyyy-MM-dd HH:mm:ss}");
            }
            var token = StringUtil.GetString(54);
            await redisUtil.SetAsync(token, data,DateTime.Now.AddMinutes(35));
            HttpContext.Response.Cookies.Append("id",$"{data.Id}");
            HttpContext.Response.Cookies.Append("user",JsonConvert.SerializeObject(data));
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
            return new OkObjectResult( await redisUtil.DeleteAsync(token)?"退出登录成功": "退出登录失败");
        }
    }
}
