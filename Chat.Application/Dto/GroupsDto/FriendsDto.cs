using Chat.Code.Entities.Users;
using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto.GroupsDto
{
    /// <summary>
    /// 好友列表
    /// </summary>
    public class FriendsDto
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Key { get; set; }
        public Guid Id { get; set; }
        /// <summary>
        /// 聊天链接id
        /// </summary>
        public string Receiving { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid SelfId { get; set; }
        /// <summary>
        /// 好友id
        /// </summary>
        public Guid FriendId { get; set; }
        public UserDto Self { get; set; }
        public UserDto Friend { get; set; }
    }
}
