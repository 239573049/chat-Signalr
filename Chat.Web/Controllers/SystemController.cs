using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat.Uitl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 系统信息接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        /// <summary>
        /// 获取运行系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OSVersion()
        {
            return new OkObjectResult(ServerConfig.GetServerInfo());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCpu(bool status=true)
        {
            return new OkObjectResult(CPULinuxLoadValue.QUERY_CPULOAD(status));
        }
    }
}
