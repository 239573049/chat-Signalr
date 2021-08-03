using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Web.Code.Model
{
    public class PeddleDataResponse<T>
    {
        public T Data { get; }
        public int Count { get; }

        public PeddleDataResponse(T data, int count)
        {
            Data = data;
            Count = count;
        }
    }
}
