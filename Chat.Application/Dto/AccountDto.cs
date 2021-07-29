using Chat.Code.DbEnum;
using Chat.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Dto
{
    /// <summary>
    /// 账号
    /// </summary>
    public class AccountDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AccountNumber { get; set; }
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
        /// 状态
        /// </summary>
        public string StatusName { get; set; }
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
