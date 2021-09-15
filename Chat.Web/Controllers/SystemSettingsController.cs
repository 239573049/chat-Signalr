using Chat.Uitl.Util;
using Chat.Web.Code.Model.SystemVM;
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
            var data =new Appsettings();
            data.ConnectionString = new ConnectionString() {Default=AppSettingsUtil.App("connectionString:default"),MongoDB= AppSettingsUtil.App("connectionString:mongoDB"),MongoDBData= AppSettingsUtil.App("connectionString:mongoDBData"),Redis= AppSettingsUtil.App("connectionString:redis") };
            data.CurrentLimiting = new CurrentLimiting() {count=AppSettingsUtil.GetValue<int>("currentLimiting:count"),second=AppSettingsUtil.GetValue<int>("currentLimiting:second") };
            data.FileServer = new FileServer {SingleFileMaxSize=AppSettingsUtil.App("fileServer:SingleFileMaxSize") };
            data.Oss = new Code.Model.SystemVM.Oss {accessKeyId=AppSettingsUtil.App("oss:accessKeyId"),accessKeySecret= AppSettingsUtil.App("oss:accessKeySecret"),bucketName= AppSettingsUtil.App("oss:bucketName"),endpoint= AppSettingsUtil.App("oss:endpoint"),path= AppSettingsUtil.App("oss:path") };
            data.PushTime = AppSettingsUtil.GetValue<int>("pushTime");
            data.ServiceName = AppSettingsUtil.App("serviceName");
            return new OkObjectResult(data);
        }
    }
}
