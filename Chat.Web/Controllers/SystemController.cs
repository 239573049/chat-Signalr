using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat.Uitl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Web.Code;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 系统信息接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class SystemController : ControllerBase
    {
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult OSVersion()
        //{
        //    return new OkObjectResult(new { MemInfo = ServerConfig.ReadMemInfo(), Cpu = CPULinuxLoadValue.QUERY_CPULOAD(false) });
        //}
    }
}
