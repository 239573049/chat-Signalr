using Chat.Uitl.Util;

using Cx.NetCoreUtils.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using Util;

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
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type">0(文件)|1(图片)|2（视频）|3（音频）|4（其他）</param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        [HttpPost]
        public async Task<IActionResult> Uploading(IFormFile file, sbyte type = 0)
        {
            var max = Convert.ToInt64(AppSettings.App("File:max"));
            if (file.Length > max * 1024 * 1024) throw new BusinessLogicException($"文件大于{max}MB无法上传");
            var path = AppSettings.App("File:path");
            switch (type)
            {
                case 0:
                    path += "/files";
                    break;
                case 1:
                    path += "/image";
                    break;
                case 2:
                    path += "/mp4";
                    break;
                case 3:
                    path += "/mp3";
                    break;
                default:
                    path += "/data";
                    break;
            }
            var names = file.FileName.Split(".");
            var name = $"{StringUtil.GetString(20)}{DateTime.Now:yyyyMMddHHmmss}";
            if (name.Length > 1)
            {
                name += "." + names[^1];
            }
            path = await new OssUtil().UploadingFile(path + "/" + name, file.OpenReadStream());
            return new OkObjectResult(path);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string path)
        {

            return new OkObjectResult((await new OssUtil().DeleteFile(path)) ? "删除成功" : "删除失败");
        }
    }
}
