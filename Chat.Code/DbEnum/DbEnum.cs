using System.ComponentModel;

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
    public enum UseStateEnume
    {
        /// <summary>
        /// 在线
        /// </summary>
        [Description("在线")]
        OnLine,
        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        OffLine
    }
    public enum SysytemMarkingEnum
    {
        /// <summary>
        /// 好友申请
        /// </summary>
        FriendRequest,
        /// <summary>
        /// 系统通知
        /// </summary>
        SystematicNotification,
        /// <summary>
        /// 好友申请[通过|拒绝]
        /// </summary>
        FriendRequestStatus,


    }
}
