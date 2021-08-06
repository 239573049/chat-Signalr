using System;
using System.IO;

namespace Chat.Uitl.Util
{
    public static class LinuxData
    {
        private static CPU_OCCUPY previous_cpu_occupy = null;
        private static readonly object syncobj = new();

        private class CPU_OCCUPY
        {
            public string name;
            public long user;
            public long nice;
            public long system;
            public long idle;
            public long lowait;
            public long irq;
            public long softirq;
        }
        public static int QUERY_CPULOAD(bool a = false)
        {
            lock (syncobj) {
                CPU_OCCUPY current_cpu_occupy = get_cpuoccupy();
                if (current_cpu_occupy == null || previous_cpu_occupy == null) {
                    previous_cpu_occupy = current_cpu_occupy;
                    return 0;
                }
                try {
                    long od = (previous_cpu_occupy.user + previous_cpu_occupy.nice + previous_cpu_occupy.system + previous_cpu_occupy.idle + previous_cpu_occupy.lowait + previous_cpu_occupy.irq + previous_cpu_occupy.softirq);//第一次(用户+优先级+系统+空闲)的时间再赋给od
                    long nd = (current_cpu_occupy.user + current_cpu_occupy.nice + current_cpu_occupy.system + current_cpu_occupy.idle + current_cpu_occupy.lowait + current_cpu_occupy.irq + current_cpu_occupy.softirq);//第二次(用户+优先级+系统+空闲)的时间再赋给od

                    double sum = nd - od;
                    double idle = current_cpu_occupy.idle - previous_cpu_occupy.idle;
                    double cpu_use = idle / sum;

                    if (!a) {
                        idle = current_cpu_occupy.user + current_cpu_occupy.system + current_cpu_occupy.nice - previous_cpu_occupy.user - previous_cpu_occupy.system - previous_cpu_occupy.nice;
                        cpu_use = idle / sum;
                    }

                    cpu_use = (cpu_use * 100) / Environment.ProcessorCount;
                    return (int)cpu_use;
                }
                finally {
                    previous_cpu_occupy = current_cpu_occupy;
                }
            }
        }

        private static string ReadArgumentValue(StreamReader sr)
        {
            string values = null;
            if (sr != null) {
                while (!sr.EndOfStream) {
                    char ch = (char)sr.Read();
                    if (ch == ' ') {
                        if (values == null) {
                            continue;
                        }
                        break;
                    }
                    values += ch;
                }
            }
            return values;
        }

        private static long ReadArgumentValueInt64(StreamReader sr)
        {
            string s = ReadArgumentValue(sr);
            if (string.IsNullOrEmpty(s)) {
                return 0;
            }
            long.TryParse(s, out long r);
            return r;
        }

        private static CPU_OCCUPY get_cpuoccupy()
        {
            string path = "/proc/stat";
            if (!File.Exists(path)) {
                return null;
            }
            FileStream stat;
            try {
                stat = File.OpenRead(path);
                if (stat == null) {
                    return null;
                }
            }
            catch (Exception) {
                return null;
            }
            using StreamReader sr = new(stat); CPU_OCCUPY occupy = new();
            try {
                occupy.name = ReadArgumentValue(sr);
                occupy.user = ReadArgumentValueInt64(sr);
                occupy.nice = ReadArgumentValueInt64(sr);
                occupy.system = ReadArgumentValueInt64(sr);
                occupy.idle = ReadArgumentValueInt64(sr);
                occupy.lowait = ReadArgumentValueInt64(sr);
                occupy.irq = ReadArgumentValueInt64(sr);
                occupy.softirq = ReadArgumentValueInt64(sr);
            }
            catch (Exception) {
                return null;
            }
            return occupy;
        }


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
                    memInfo.Total = string.IsNullOrEmpty(total) ? 0 : int.Parse(total) / 1024;
                }
                else if (item.StartsWith("MemAvailable:")) {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var availble = tt[1].Trim().Split(" ")[0];
                    memInfo.Available = string.IsNullOrEmpty(availble) ? 0 : int.Parse(availble) / 1024;
                }
                if (count >= 2) break;
            }
            memInfo.Usage = Convert.ToInt32((memInfo.Total - memInfo.Available) / memInfo.Total * 100);
            return memInfo;
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
}
