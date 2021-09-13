using Chat.Uitl.Util;
using Chat.Web.Code;
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
            Oss oss = new()
            {
                accessKeyId = AppSettingsUtil.App("oss:accessKeyId"),
                accessKeySecret = AppSettingsUtil.App("oss:accessKeySecret"),
                bucketName = AppSettingsUtil.App("oss:bucketName"),
                endpoint = AppSettingsUtil.App("oss:endpoint"),
                path = AppSettingsUtil.App("oss:path")
            };
            var datas = await oss.UploadingFile($"file/{StringUtil.GetString(10)}{file.FileName}", file.OpenReadStream());
            return new OkObjectResult(new { path =$"{oss.path}/{datas}",key= datas });
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string key)
        {
            Oss oss = new()
            {
                accessKeyId = AppSettingsUtil.App("oss:accessKeyId"),
                accessKeySecret = AppSettingsUtil.App("oss:accessKeySecret"),
                bucketName = AppSettingsUtil.App("oss:bucketName"),
                endpoint = AppSettingsUtil.App("oss:endpoint"),
                path = AppSettingsUtil.App("oss:path")
            };
            return new OkObjectResult(await oss.DeleteFile(key)?"删除成功":"删除失败");
        }
    }
}
