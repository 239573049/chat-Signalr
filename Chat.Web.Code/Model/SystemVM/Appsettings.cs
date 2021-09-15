using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Web.Code.Model.SystemVM
{
    public class Appsettings
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public ConnectionString ConnectionString { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 限流
        /// </summary>
        public CurrentLimiting CurrentLimiting { get; set; }
        /// <summary>
        /// 文件设置
        /// </summary>
        public FileServer FileServer { get; set; }
        /// <summary>
        /// 阿里云oss
        /// </summary>
        public Oss Oss { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public int PushTime { get; set; }
    }
    public class Oss
    {
        /// <summary>
        /// endpoint
        /// </summary>
        public string endpoint { get; set; }
        /// <summary>
        /// accessKeyId
        /// </summary>
        public string accessKeyId { get; set; }
        /// <summary>
        /// accessKeySecret
        /// </summary>
        public string accessKeySecret { get; set; }
        /// <summary>
        /// bucketName
        /// </summary>
        public string bucketName { get; set; }
        /// <summary>
        /// 下载路径前缀
        /// </summary>
        public string path { get; set; }
    }
    public class FileServer
    {
        /// <summary>
        /// 上传限制大小（MB）
        /// </summary>
        public string SingleFileMaxSize { get; set; }
    }
    public class CurrentLimiting
    {
        /// <summary>
        /// 每多少秒
        /// </summary>
        public int second { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public int count { get; set; }
    }
    public class ConnectionString
    {
        /// <summary>
        /// 数据库地址
        /// </summary>
        public string Default { get; set; }
        /// <summary>
        /// redis地址
        /// </summary>
        public string Redis { get; set; }
        /// <summary>
        /// MongoDb地址
        /// </summary>
        public string MongoDB { get; set; }
        /// <summary>
        /// MongoDb数据库名称
        /// </summary>
        public string MongoDBData { get; set; }
    }
}
