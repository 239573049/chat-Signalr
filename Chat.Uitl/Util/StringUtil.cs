using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public class StringUtil
    {
        /// <summary>
        /// 获取字符串数字随机
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetString(int n)
        {
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new();
            var rd = new Random();
            for (int i = 0; i < n; i++) sb.Append(str.Substring(rd.Next(0, str.Length), 1));
            return sb.ToString();
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetStrings(int n)
        {
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_+<>:?|\\`~*-";
            StringBuilder sb = new();
            var rd = new Random();
            for (int i = 0; i < n; i++) sb.Append(str.Substring(rd.Next(0, str.Length), 1));
            return sb.ToString();
        }
        /// <summary>
        /// 转base64
        /// </summary>
        /// <returns></returns>
        public static string Transition(byte[] data)
        {
            return Base64.ToBase64String(data);
        }
        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decode(byte[] data)
        {
            return Base64.Decode(data);
        }
    }
}
