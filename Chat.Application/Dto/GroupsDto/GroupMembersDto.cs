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
        public Guid Id { get; set; }
        /// <summary>
        /// 群id
        /// </summary>
        public Guid GroupDataId { get; set; }
        public GroupDataDto GroupData { get; set; }
        /// <summary>
        /// 群成员id
        /// </summary>
        public Guid SelfId { get; set; }
    }
}
