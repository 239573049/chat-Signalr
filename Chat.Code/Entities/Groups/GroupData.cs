using Chat.Code.Entities.User;
using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Code.Entities.Groups
{
    /// <summary>
    /// 群
    /// </summary>
    public class GroupData : EntityWithAllBaseProperty
    {
        /// <summary>
        /// 群名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Receiving{ get; set; }
        /// <summary>
        /// 群头像
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 公告
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid SelfId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Account Self { get; set; }
    }
}
