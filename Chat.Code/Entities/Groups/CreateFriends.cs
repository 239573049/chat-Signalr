using Chat.Code.DbEnum;
using Chat.Code.Entities.User;
using Chat.Core.Base;
using System;

namespace Chat.Code.Entities.Groups
{
    /// <summary>
    /// 申请添加好友
    /// </summary>
    public class CreateFriends: EntityWithAllBaseProperty
    {
        /// <summary>
        /// 发起人
        /// </summary>
        public Guid InitiatorId { get; set; }
        /// <summary>
        /// 被邀请人
        /// </summary>
        public Guid BeInvitedId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CreateFriendsEnum CreateFriendsEnum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public Account Initiator{ get; set; }
        public Account BeInvited { get; set; }
    }
}
