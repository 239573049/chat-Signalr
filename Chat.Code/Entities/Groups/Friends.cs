using Chat.Code.Entities.Users;
using Chat.Core.Base;
using System;
namespace Chat.Code.Entities.Groups
{
    /// <summary>
    /// 好友列表
    /// </summary>
    public class Friends : EntityWithAllBaseProperty
    {
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
        public User Self { get; set; }
        public User Friend { get; set; }
    }
}
