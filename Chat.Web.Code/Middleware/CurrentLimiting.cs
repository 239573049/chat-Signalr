using Chat.Uitl.Util;
using Cx.NetCoreUtils.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
<<<<<<< HEAD
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
=======
            var data =await redisUtil.GetAsync<Data>(targetInfo);
            if (data==null) {
                var now = DateTime.Now.AddSeconds(seconds);
                data = new Data
                {
                    Count = 1,
                    Time = now
                };
                await redisUtil.SetAsync(targetInfo, data, now);
            }
            else {
                if (data.Count>=count) {
                    filterContext.Result = new ObjectResult(new ModelStateResult($"ip：{filterContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4()} 超出访问{count}次限制，请稍后请求", 413));
                }
                else {
                   data.Count++;
                   await redisUtil.SetAsync(targetInfo, data, data.Time);
>>>>>>> c26aefd1cedbdbb5d8361719a5dabc59836bc54b
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
