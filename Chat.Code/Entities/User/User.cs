using Chat.Code.DbEnum;
using Chat.Core.Base;
using System;

namespace Chat.Code.Entities.Users
{
    /// <summary>
    /// 账号
    /// </summary>
    public class User: EntityWithAllBaseProperty
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserNumber { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWrod { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusEnum Status { get; set; }
        /// <summary>
        /// 账号在线状态
        /// </summary>
        public UseStateEnume UseState { get; set; } = UseStateEnume.OffLine;
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
        /// <summary>
        /// 权限区分
        /// </summary>
        public PowerEnum Power { get; set; }
        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime? Freezetime { get; set; }
    }
}
