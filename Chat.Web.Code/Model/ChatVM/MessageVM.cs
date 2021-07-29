using System;
using static Chat.Web.Code.EnumWeb.EnumWeb;

namespace Chat.Web.Code.Model.ChatVM
{
    /// <summary>
    /// 数据模型
    /// </summary>
    public class MessageVM
    {
        /// <summary>
        /// 发送人id
        /// </summary>
        public Guid SendId { get; set; }
        /// <summary>
        /// 接收人id
        /// </summary>
        public string Receiving { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 传输类型
        /// </summary>
        public ChatMessageEnum ChatMessageEnum{ get; set; }
    }
}
