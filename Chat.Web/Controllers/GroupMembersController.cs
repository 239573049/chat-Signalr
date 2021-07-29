using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 群列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        public GroupMembersController(
            )
        {

        }
    }
}
