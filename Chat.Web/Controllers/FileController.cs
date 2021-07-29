using Chat.Uitl.Util;
using Cx.NetCoreUtils.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 文件接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        /// <summary>
        /// 上传文件接口
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Uploading(IFormFile file)
        {
            if (file.Length > (50 * 1024 * 1024)) return new ModelStateResult("上传文件大小不能超过50MB");
            Oss oss = new Oss
            {
                accessKeyId = AppSettings.App("oss:accessKeyId"),
                accessKeySecret = AppSettings.App("oss:accessKeySecret"),
                bucketName = AppSettings.App("oss:bucketName"),
                endpoint = AppSettings.App("oss:endpoint"),
                path = AppSettings.App("oss:path")
            };
            return new OkObjectResult(await oss.UploadingFile($"file/{StringUtil.GetString(10)}{file.FileName}", file.OpenReadStream()));
        }

    }
}
