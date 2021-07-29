﻿using Chat.Code.Entities.User;
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
        public AccountDto Self { get; set; }
        public AccountDto Friend { get; set; }
    }
}
