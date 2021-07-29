using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
