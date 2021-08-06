using System;

namespace Chat.Web.Code.Model.SystemVM
{
    public class SystemData
    {
        /// <summary>
        /// cpu使用率
        /// </summary>
        public int Cpu { get; set; }
        /// <summary>
        /// 已使用运行内存
        /// </summary>
        public int Usage { get; set; }
        /// <summary>
        /// 总运行内存
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 剩余内存
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// 服务器运行时间
        /// </summary>
        public TimeSpan SystemUpTime { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        public string SystemOs { get; set; }
    }
}
