using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public interface IRedisUtil
    {
        string Get(string key);
        void Set(string key, object t, DateTime? expiresSec=null);
        T Get<T>(string key) where T : new();
        Task<string> GetAsync(string key);
        Task SetAsync(string key,object t,DateTime? expiresSec=null);
        Task<T> GetAsync<T>(string key);
        bool SetDate(string key,DateTime date);
        Task<bool> SetDateAsync(string key,DateTime date);
        Task<bool> DeleteAsync(string key);
        bool Delete(string key);
    }
}
