using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Chat.Uitl.Util;
using Chat.Application.Dto;

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
            var path = httpContext.HttpContext.Request.Path.Value;
            var authorizations = new RedisUtil().Get<AccountDto>(authorization);
            if (authorizations == null) throw new BusinessLogicException(401, "请先登录账号");



        }

    }
}
