using Chat.Uitl.Util;
using Cx.NetCoreUtils.Common;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

namespace Chat.Web.Code.Middleware
{
    public  class CurrentLimiting: ActionFilterAttribute
    {
        private readonly IRedisUtil redisUtil;
        public CurrentLimiting(
            IRedisUtil redisUtil
            )
        {
            this.redisUtil = redisUtil;
        }
        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var seconds = AppSettings.GetValue<int>("CurrentLimiting:second");
            var count = AppSettings.GetValue<int>("CurrentLimiting:count");
            var targetInfo = $"{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}";
            var data =await redisUtil.GetAsync<Data>(targetInfo);
            if (data==null) {
               await redisUtil.SetAsync(targetInfo, 1,DateTime.Now.AddSeconds(seconds));
            }
            else {
                if (count >= data.count) {
                    filterContext.Result = new ObjectResult(new ModelStateResult($"ip：{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()} 超出访问{count}次限制，请稍后请求", 413));
                }
                else {
                   await redisUtil.SetAsync(targetInfo, data.count++, DateTime.Now.AddSeconds(seconds));
                }
            }
            base.OnActionExecuting(filterContext);
        }
        public class Data
        {
            public DateTime Time { get; set; }
            public int count { get; set; }
        }
    }
}
