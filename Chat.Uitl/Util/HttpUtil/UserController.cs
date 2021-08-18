using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util.HttpUtil
{
    public class UsersController: ControllerBase
    {
        public Guid UserId{ get{
                HttpContext.Request.Cookies.TryGetValue("id", out string id);
                if (string.IsNullOrEmpty(id)) throw new BusinessLogicException(401,"请先登录账号");
                var ids = Guid.Parse(id);
                return ids;
            } }
    }
}
