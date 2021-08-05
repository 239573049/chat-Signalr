using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Web.Code.EnumWeb
{
    public class EnumWeb
    {
        public enum ChatMessageEnum{
            /// <summary>
            /// 文本
            /// </summary>
            Text,
            /// <summary>
            /// 文件
            /// </summary>
            File,
            /// <summary>
            /// 图片
            /// </summary>
            Image,
            /// <summary>
            /// 语音
            /// </summary>
            Voice,
            /// <summary>
            /// 视频
            /// </summary>
            Video
        }
        public  enum ChatSystemEnum 
        { 
            /// <summary>
            /// 系统信息
            /// </summary>
            SystemData,
            /// <summary>
            /// 系统提示
            /// </summary>
            SystemPrompt,
            /// <summary>
            /// 系统信息
            /// </summary>
            Systemmessage

        }
    }
}
