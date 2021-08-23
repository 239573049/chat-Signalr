using MongoDB.Bson.Serialization.Attributes;
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
        public string HeadPortrait { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 传输类型
        /// </summary>
        public ChatMessageEnum Marking { get; set; }
        [BsonId]
        public Guid Key { get; set; }
    }
}
