using Chat.Web.Code.Model.SystemVM;

using Cx.NetCoreUtils.Extensions;

using Microsoft.AspNetCore.Mvc;

using System;

using Util;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        /// <summary>
        /// 获取系统设置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSystemSettings()
        {
            var data = new Appsettings
            {
                ConnectionString = new ConnectionString() { Default = AppSettings.App("connectionString:default").MD5Decrypt(), Redis = AppSettings.App("connectionString:redis") },
                CurrentLimiting = new CurrentLimiting() { count = Convert.ToInt32(AppSettings.App("currentLimiting:count")), second = Convert.ToInt32(AppSettings.App("currentLimiting:second")) },
                FileServer = new FileServer { SingleFileMaxSize = AppSettings.App("fileServer:SingleFileMaxSize") },
                Oss = new Code.Model.SystemVM.Oss { accessKeyId = AppSettings.App("oss:accessKeyId"), accessKeySecret = AppSettings.App("oss:accessKeySecret"), bucketName = AppSettings.App("oss:bucketName"), endpoint = AppSettings.App("oss:endpoint"), path = AppSettings.App("oss:path") },
                PushTime = Convert.ToInt32(AppSettings.App("pushTime")),
                ServiceName = AppSettings.App("serviceName")
            };
            return new OkObjectResult(data);
        }
        /// <summary>
        /// 设置系统信息
        /// </summary>
        /// <param name="appsettings"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SetSystemSettings(Appsettings appsettings)
        {
            //if (appsettings != null) {
            //    AppSettings.SetValue(appsettings.ConnectionString.Default.MD5Encrypt(), "connectionString:default");
            //    AppSettings.SetValue(appsettings.ConnectionString.Redis, "connectionString:redis");
            //    AppSettings.SetValue(appsettings.CurrentLimiting.count.ToString(), "currentLimiting:count");
            //    AppSettings.SetValue(appsettings.CurrentLimiting.second.ToString(), "currentLimiting:second");
            //    AppSettings.SetValue(appsettings.FileServer.SingleFileMaxSize, "fileServer:SingleFileMaxSize");
            //    AppSettings.SetValue(appsettings.Oss.accessKeyId, "oss:accessKeyId");
            //    AppSettings.SetValue(appsettings.Oss.accessKeySecret, "oss:accessKeySecret");
            //    AppSettings.SetValue(appsettings.Oss.bucketName, "oss:bucketName");
            //    AppSettings.SetValue(appsettings.Oss.endpoint, "oss:endpoint");
            //    AppSettings.SetValue(appsettings.Oss.path, "oss:path");
            //    AppSettings.SetValue(appsettings.PushTime.ToString(), "pushTime");
            //    AppSettings.SetValue(appsettings.ServiceName, "serviceName");
            //}
            return new OkObjectResult("");
        }
    }
}
