using Chat.Application.Dto.GroupsDto;
using Chat.Code.DbEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Web.Code.Model.GroupsVM
{
    public class GroupDataVM
    {

        /// <summary>
        /// 索引
        /// </summary>
        public int Key { get; set; }
        public Guid Id { get; set; }
        /// <summary>
        /// 群id
        /// </summary>
        public Guid ChatId { get; set; }
        /// <summary>
        /// 聊天链接id
        /// </summary>
        public string Receiving { get; set; }
        /// <summary>
        /// 未读信息
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 是否是群
        /// </summary>
        public bool IsGroup { get; set; }
        public object Data { get; set; }
        /// <summary>
        /// 群成员id
        /// </summary>
        public Guid SelfId { get; set; }
    }
}
