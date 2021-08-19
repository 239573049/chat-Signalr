using Chat.Uitl.Util;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Model.ChatVM;
using Chat.Web.Code.Model.SystemVM;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Chat.Uitl.Util.LinuxData;
using static Chat.Web.Code.EnumWeb.EnumWeb;

namespace Chat.Web.Code.Job
{
    public class Jobs : IJob
    {
        public static IHubContext<ChatHub> HubContext { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                if (ChatHub.UserData.IsEmpty) return;
                var data = Dispose();
                var systemData = new SystemMassageVM
                {
                    Data = data,
                    Marking = ChatSystemEnum.SystemData
                };
                HubContext.Clients.All.SendAsync("SystemData", systemData);
            });
        }
        public static SystemData Dispose()
        {
            var data = new SystemData();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                var use = ReadMemInfo();
                data.Available = use.Available;
                data.Total = use.Total;
                data.Usage = use.Usage;
                data.Cpu = QUERY_CPULOAD(false);
                data.SystemOs = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                data.Available=WinData.GetRAM();
                data.SystemOs = "Windows";
                data.SystemUpTime = WinData.GetSystemUpTime();
                data.Total = WinData.GetMemory();
                data.Cpu = WinData.GetCpuUsage();
                data.Usage = Convert.ToInt32((data.Total - data.Available) / data.Total * 100);
            }
            return data;
        }
    }
}
