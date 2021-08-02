using Chat.Code.DbEnum;
using Chat.Code.Entities.Users;
using Chat.Core.Base;
using System;

namespace Chat.Application.Dto.GroupsDto
{
    /// <summary>
    /// 申请添加好友
    /// </summary>
    public class CreateFriendsDto
    {
        public Guid Id { get; set; }
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
        public UserDto Initiator{ get; set; }
        public UserDto BeInvited { get; set; }
    }
}
