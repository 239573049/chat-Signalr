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
        Guid GetId();
        string GetToken();
    }
}
