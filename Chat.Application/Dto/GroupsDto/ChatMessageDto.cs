using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto.GroupsDto
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
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
        public sbyte Marking { get; set; }
    }
}
