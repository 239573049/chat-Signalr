using System;
using System.IO;

namespace Chat.Uitl.Util
{
    public static class CPULinuxLoadValue
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

    }
}
