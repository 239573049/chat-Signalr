using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 好友列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        public FriendsController(
            )
        {

        }
    }
}
