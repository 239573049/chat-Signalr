using Chat.Uitl.Util;
using Cx.NetCoreUtils.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

namespace Chat.Web.Code.Middleware
{
    public  class CurrentLimiting: ActionFilterAttribute
    {
        private readonly IMemoryCache memory;
        public CurrentLimiting(
            IMemoryCache memory
            )
        {
            this.memory = memory;
        }
        public  override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var seconds = AppSettings.GetValue<int>("CurrentLimiting:second");
            var count = AppSettings.GetValue<int>("CurrentLimiting:count");
            var targetInfo = $"{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}";
            if (!memory.TryGetValue(targetInfo, out Data data)) {
                data = new Data
                {
                    Count = 1,
                    Time = DateTime.Now.AddSeconds(seconds)
                };
                memory.Set(targetInfo, data, data.Time);
            }
            else {
                if (data.Count>count)
                {
                    filterContext.Result = new ObjectResult(new ModelStateResult($"ip：{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}，请求速度过快，超过每{seconds}秒{count}次限制，请稍后请求", 413));//这边是超过设置的请求数量进行返回
                }
                else {
                   data.Count++;
                    memory.Set(targetInfo, data, data.Time);
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
