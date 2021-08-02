using System.Collections.Generic;
using System.Linq;
using Chat.Application.Dto;
using Chat.Web.Code.Model;
using NPOI.SS.Formula.Functions;

namespace Chat.Web.Code
{
    public class SerialNumber
    {
        public static IList<T> GetList<T>(IList<T> ts,int pageNo,int pageSize) where T : SerialNumberDto
        {
            var i = (pageNo - 1) * pageSize + 1;
            ts.ToList().ForEach(a => a.Key = i++);
            return ts;
        }
    }
}
