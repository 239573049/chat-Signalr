using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Chat.Uitl.Util
{
    public class WinData
    {
        static readonly PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
        static readonly PerformanceCounter ramCounter = new("Memory", "Available MBytes");
        static readonly PerformanceCounter uptime = new("System", "System Up Time");


        public static bool GetInternetAvilable()
        {
            bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            return networkUp;
        }

        public static TimeSpan GetSystemUpTime()
        {
            uptime.NextValue();
            TimeSpan ts = TimeSpan.FromSeconds(uptime.NextValue());
            return ts;
        }

        public static long GetMemory()
        {
            string str = null;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get()) {
                str = objMgmt["totalphysicalmemory"].ToString();
            }
            return Convert.ToInt64(str)/(1024 * 1024);
        }

        public static int GetCpuUsage()
        {
            return Convert.ToInt32(cpuCounter.NextValue());
        }

        public static int GetRAM()
        {
            return Convert.ToInt32(ramCounter.NextValue());
        }
    }
}
