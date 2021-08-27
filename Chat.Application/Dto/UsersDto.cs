using Chat.Code.DbEnum;
using Chat.Web.Code.Model;
using System;

namespace Chat.Application.Dto
{
    /// <summary>
    /// 账号
    /// </summary>
    public class UsersDto: SerialNumberDto
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
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
    }
}
