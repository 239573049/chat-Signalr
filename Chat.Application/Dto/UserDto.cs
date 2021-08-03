using Chat.Code.DbEnum;
using Chat.Web.Code.Model;
using System;

namespace Chat.Application.Dto
{
    /// <summary>
    /// 账号
    /// </summary>
    public class UserDto: SerialNumberDto
    {
        public Guid Id { get; set; }
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
        public string StatusCode { get; set; }
        /// <summary>
        /// 账号在线状态
        /// </summary>
        public UseStateEnume UseState { get; set; } = UseStateEnume.OffLine;
        public string UseStateCode { get; set; }
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
