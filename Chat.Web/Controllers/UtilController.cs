using Cx.NetCoreUtils.Extensions;
using Microsoft.AspNetCore.Mvc;
using static Cx.NetCoreUtils.Filters.GlobalModelStateValidationFilter;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 工具
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MD5Decrypt(string data)
        {
            if (string.IsNullOrEmpty(data)) return new ModelStateResult(string.Empty);
            return new OkObjectResult(data.MD5Decrypt());
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MD5Encrypt(string data)
        {
            if(string.IsNullOrEmpty(data))return new ModelStateResult(string.Empty) ;
            return new OkObjectResult(data.MD5Encrypt());
        }
    }
}
