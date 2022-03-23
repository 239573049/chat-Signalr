using Chat.Uitl.Util;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Model.ChatVM;
using Chat.Web.Code.Model.SystemVM;

using Cx.NetCoreUtils.Common;

using Microsoft.AspNetCore.SignalR;

using System;
using System.Runtime.InteropServices;
using System.Threading;

using static Chat.Uitl.Util.LinuxData;
using static Chat.Web.Code.EnumWeb.EnumWeb;
namespace Chat.Web.Code
{
    public class TimedTask
    {
        public static IHubContext<ChatHub> HubContext { get; set; }
        public static IRedisUtil redisUtil { get; set; }
        public static bool IsStatus { get; set; } = false;
        public static void Tasks()
        {
            new Thread(async delegate() {
                while (IsStatus) {
                    var pushTime = AppSettings.GetValue<int>("pushTime");
                    var data = Dispose();
                    var systemData = new SystemMassageVM
                    {
                        Data = data,
                        Key=Guid.NewGuid(),
                        Marking = ChatSystemEnum.SystemData
                    };
                    var receiving =(await redisUtil.SMembersAsync<string>("admin"));
                    foreach (var d in receiving) {
                        await HubContext.Clients.Client(d).SendAsync("SystemData", systemData);
                    }
                    Thread.Sleep((int)pushTime);
                }
            }).Start();
        }
        public static SystemData Dispose()
        {
            var data = new SystemData
            {
                OnLine = ChatHub.UserData.Count
            };
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                var use = ReadMemInfo();
                data.Available = use.Available;
                data.Total = use.Total;
                data.Usage = use.Usage;
                data.Cpu = QUERY_CPULOAD(false);
                data.SystemOs = "Linux";
            }
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            //    data.Available = WinData.GetRAM();
            //    data.SystemOs = "Windows";
            //    data.SystemUpTime = WinData.GetSystemUpTime();
            //    data.Total = WinData.GetMemory();
            //    data.Cpu = WinData.GetCpuUsage();
            //    data.Usage = Convert.ToInt32((data.Total - data.Available) / data.Total * 100);
            //}
            data.ServiceName = (string)AppSettings.GetValue<string>("ServiceName");
            return data;
        }
    }
}
