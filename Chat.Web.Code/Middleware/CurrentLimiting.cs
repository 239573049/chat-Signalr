using Chat.Uitl.Util;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;

using Util;

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
            var seconds = Convert.ToInt32(AppSettings.App("currentLimiting:second"));
            var count =Convert.ToInt32(AppSettings.App("currentLimiting:count"));
            var targetInfo = $"{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}";
            var data =await redisUtil.GetAsync<Data>(targetInfo);
            if (data==null) {
                var now = DateTime.Now.AddSeconds((int)seconds);
                data = new Data
                {
                    Count = 1,
                    Time = now
                };
               await redisUtil.SetAsync(targetInfo,data, now);
            }
            else {
                if (data.Count>=(int)count) {
                    filterContext.Result = new ObjectResult(new ModelStateResult($"ip：{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()} 超出访问{count}次限制，请稍后请求", 413));
                }
                else {
                   data.Count++;
                   await redisUtil.SetAsync(targetInfo, data, data.Time);
                }
            }
            base.OnActionExecuting(filterContext);
        }
        public class Data
        {
            public DateTime Time { get; set; }
            public int Count { get; set; }
        }
    }
}
