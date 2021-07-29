using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public class PrincipalAccessor: IPrincipalAccessor
    {
        private readonly IRedisUtil redisUtil;
        private readonly IHttpContextAccessor accessor;
        public PrincipalAccessor(
            IRedisUtil redisUtil,
            IHttpContextAccessor accessor
            )
        {
            this.accessor = accessor;
            this.redisUtil = redisUtil;
        }

        public Guid GetId()
        {
            accessor.HttpContext.Request.Cookies.TryGetValue("id",out string id);
            return Guid.Parse(id);
        }

        public string GetToken()
        {
            return accessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        public async Task<T> GetUser<T>()
        {
            accessor.HttpContext.Request.Cookies.TryGetValue("Authorization", out string authorization);
            return await redisUtil.GetAsync<T>(authorization);
        }
    }
}
