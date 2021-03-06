using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Chat.Uitl.Util;
using Chat.Application.Dto;
using Chat.Code.DbEnum;

namespace Chat.Web.Code
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 权限拦截
        /// </summary>
        /// <param name="httpContext"></param>
        public override void OnActionExecuting(ActionExecutingContext httpContext)
        {

            string authorization = httpContext.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorization)) throw new BusinessLogicException(401, "请先登录账号");
            var path = httpContext.HttpContext.Request.Path.Value;
            authorization = authorization.Split("Bearer ")[1];
            var userDto = new RedisUtil().Get<UserDto>(authorization);
            if (userDto == null) throw new BusinessLogicException(401, "请先登录账号");
            if (userDto.Status != StatusEnum.Start) throw new BusinessLogicException($"账号无法使用账号状态：{EnumExtensionUtil.GetEnumStringVal(userDto.Status)}");

        }

    }
}
