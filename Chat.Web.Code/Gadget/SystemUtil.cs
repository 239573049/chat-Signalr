using Chat.Web.Code.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Web.Code.Gadget
{
    public class SystemUtil
    {
        public static SystemDataVM GetSystem()
        {
            var data = new SystemDataVM();
            if (OperatingSystem.IsLinux())data.System = "Linux";
            else if (OperatingSystem.IsWindows())data.System = "Window";
            else if (OperatingSystem.IsAndroid())data.System = "Android";
            else if (OperatingSystem.IsIOS())data.System = "IOS";
            else if (OperatingSystem.IsFreeBSD())data.System = "FreeBSD";
            else if (OperatingSystem.IsMacOS())data.System = "MacO";
            else if (OperatingSystem.IsWatchOS())data.System = "WatchOS";
            else if (OperatingSystem.IsTvOS())data.System = "TvOS";

            if (Environment.Is64BitOperatingSystem)
            {
                data.Bit = 64;
            }
            return data;
        }

    }
}
