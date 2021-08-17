using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto.GroupsDto
{
    /// <summary>
    /// 群列表
    /// </summary>
    public class GroupMembersDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Key { get; set; }
        public Guid Id { get; set; }
        /// <summary>
        /// 群id
        /// </summary>
        public Guid GroupDataId { get; set; }
        /// <summary>
        /// 未读信息
        /// </summary>
        public int Count { get; set; }
        public GroupDataDto GroupData { get; set; }
        /// <summary>
        /// 群成员id
        /// </summary>
        public Guid SelfId { get; set; }
        public object Self{ get; set; }
    }
}
