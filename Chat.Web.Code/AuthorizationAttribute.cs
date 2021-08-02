using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Chat.Uitl.Util;
using Chat.Application.Dto;
<<<<<<< HEAD
using Chat.Code.DbEnum;
=======
>>>>>>> c26aefd1cedbdbb5d8361719a5dabc59836bc54b

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

            httpContext.HttpContext.Request.Cookies.TryGetValue("Authorization", out string authorization);
            if(string.IsNullOrEmpty(authorization)) throw new BusinessLogicException(401, "请先登录账号");
            var path = httpContext.HttpContext.Request.Path.Value;
<<<<<<< HEAD
            var userDto = new RedisUtil().Get<UserDto>(authorization);
            if (userDto == null) throw new BusinessLogicException(401, "请先登录账号");
            if (userDto.Status != StatusEnum.Start) throw new BusinessLogicException($"账号无法使用账号状态：{EnumExtensionUtil.GetEnumStringVal(userDto.Status)}");
=======
            var authorizations = new RedisUtil().Get<AccountDto>(authorization);
            if (authorizations == null) throw new BusinessLogicException(401, "请先登录账号");


>>>>>>> c26aefd1cedbdbb5d8361719a5dabc59836bc54b

        }

    }
}
