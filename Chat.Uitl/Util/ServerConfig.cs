using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Chat.Uitl.Util
{
    public class ServerConfig
    {
        /// <summary>
        /// 读取内存信息
        /// </summary>
        /// <returns></returns>
        public static MemInfo ReadMemInfo()
        {
            MemInfo memInfo = new();
            const string CPU_FILE_PATH = "/proc/meminfo";
            var mem_file_info = File.ReadAllText(CPU_FILE_PATH);
            var lines = mem_file_info.Split(new[] { '\n' });
            int count = 0;
            foreach (var item in lines) {
                if (item.StartsWith("MemTotal:")) {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var total = tt[1].Trim().Split(" ")[0];
                    memInfo.Total= string.IsNullOrEmpty(total)?0: int.Parse(total) / 1024;
                }
                else if (item.StartsWith("MemAvailable:")) {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var availble = tt[1].Trim().Split(" ")[0];
                    memInfo.Available = string.IsNullOrEmpty(availble) ? 0 : int.Parse(availble)/1024;
                }
                if (count >= 2) break;
            }
            memInfo.Usage = Convert.ToInt32((memInfo.Total - memInfo.Available) / memInfo.Total * 100);
            return memInfo;
        }
    }
    public class MemInfo
    {
        /// <summary>
        /// 总计内存大小
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 可用内存大小
        /// </summary>
        public decimal Available { get; set; }
        public int Usage { get; set; }
    }
}
