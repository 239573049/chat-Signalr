using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chat.Web.Code.EnumWeb.EnumWeb;

namespace Chat.Web.Code.Model.ChatVM
{
    public class SystemMassageVM
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiving { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data{ get; set; }
        /// <summary>
        /// 信息标识
        /// </summary>
        public ChatSystemEnum Marking { get; set; }
    }
}
