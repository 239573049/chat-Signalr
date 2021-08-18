using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public interface IPrincipalAccessor
    {
        Task<T> GetUser<T>();
        Task<T> Get<T>(string key);
        Guid GetId();
        string GetToken();
        T GetUserDto<T>();
    }
}
