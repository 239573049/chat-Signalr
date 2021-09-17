using Chat.Uitl.Util;
using Chat.Web.Code.Model.SystemVM;
using Cx.NetCoreUtils.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                ConnectionString = new ConnectionString() { Default = AppSettingsUtil.App("connectionString:default").MD5Decrypt(), Redis = AppSettingsUtil.App("connectionString:redis") },
                CurrentLimiting = new CurrentLimiting() { count = (int)AppSettingsUtil.GetValue<int>("currentLimiting:count"), second = (int)AppSettingsUtil.GetValue<int>("currentLimiting:second") },
                FileServer = new FileServer { SingleFileMaxSize = AppSettingsUtil.App("fileServer:SingleFileMaxSize") },
                Oss = new Code.Model.SystemVM.Oss { accessKeyId = AppSettingsUtil.App("oss:accessKeyId"), accessKeySecret = AppSettingsUtil.App("oss:accessKeySecret"), bucketName = AppSettingsUtil.App("oss:bucketName"), endpoint = AppSettingsUtil.App("oss:endpoint"), path = AppSettingsUtil.App("oss:path") },
                PushTime = (int)AppSettingsUtil.GetValue<int>("pushTime"),
                ServiceName = AppSettingsUtil.App("serviceName")
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
            if (appsettings != null) {
                AppSettingsUtil.SetValue(appsettings.ConnectionString.Default.MD5Encrypt(), "connectionString:default");
                AppSettingsUtil.SetValue(appsettings.ConnectionString.Redis, "connectionString:redis");
                AppSettingsUtil.SetValue(appsettings.CurrentLimiting.count.ToString(), "currentLimiting:count");
                AppSettingsUtil.SetValue(appsettings.CurrentLimiting.second.ToString(), "currentLimiting:second");
                AppSettingsUtil.SetValue(appsettings.FileServer.SingleFileMaxSize, "fileServer:SingleFileMaxSize");
                AppSettingsUtil.SetValue(appsettings.Oss.accessKeyId, "oss:accessKeyId");
                AppSettingsUtil.SetValue(appsettings.Oss.accessKeySecret, "oss:accessKeySecret");
                AppSettingsUtil.SetValue(appsettings.Oss.bucketName, "oss:bucketName");
                AppSettingsUtil.SetValue(appsettings.Oss.endpoint, "oss:endpoint");
                AppSettingsUtil.SetValue(appsettings.Oss.path, "oss:path");
                AppSettingsUtil.SetValue(appsettings.PushTime.ToString(), "pushTime");
                AppSettingsUtil.SetValue(appsettings.ServiceName, "serviceName");
            }
            return new OkObjectResult("");
        }
    }
}
