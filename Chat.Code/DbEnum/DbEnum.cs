using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Code.DbEnum
{
    public enum StatusEnum
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Start = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze
    }
    public enum PowerEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Manage = 0,

        /// <summary>
        /// 普通用户
        /// </summary>

        [Description("普通用户")]
        Common
    }
    public enum CreateFriendsEnum
    {
        /// <summary>
        /// 申请中
        /// </summary>
        [Description("申请中")]
        Applying=0,
        /// <summary>
        /// 同意添加
        /// </summary>
        [Description("同意")]
        Consent,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Description("拒绝")]
        Refuse
    }
}
