using Chat.Code.Entities.Users;
using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Code.Entities.Groups
{
    /// <summary>
    /// 群列表
    /// </summary>
    public class GroupMembers : EntityWithAllBaseProperty
    {
        /// <summary>
        /// 群id
        /// </summary>
        public Guid GroupDataId { get; set; }
        public GroupData GroupData { get; set; }
        /// <summary>
        /// 群成员id
        /// </summary>
        public Guid SelfId { get; set; }
        public virtual User Self { get; set; }
    }
}
