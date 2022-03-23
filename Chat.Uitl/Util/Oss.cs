using Aliyun.OSS;
using Cx.NetCoreUtils.Common;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Util
{
    public class OssUtil
    {
        public string endpoint
        {
            get
            {
                return AppSettings.App("oss:endpoint")!;

            }
        }
        public string accessKeyId
        {
            get
            {
                return AppSettings.App("oss:accessKeyId")!;
            }
        }
        public string accessKeySecret
        {
            get
            {
                return AppSettings.App("oss:accessKeySecret")!;
            }
        }
        public string bucketName { get { return AppSettings.App("oss:bucketName")!; } }
        public string path { get { return AppSettings.App("oss:path")!; } }
        public async Task<string> UploadingFile(string path, Stream stream)
        {
            return await Task.Run(() =>
            {
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                client.PutObject(bucketName, path, stream);
                return $"{this.path}/{path}";
            });
        }
        public async Task<bool> DeleteFile(string path)
        {
            return await Task.Run(() =>
            {
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                client.DeleteObject(bucketName, path.Replace(this.path, ""));
                return true;
            });
        }
        public bool DeletesFile(List<string> paths)
        {
            try
            {
                paths.ForEach(path =>
                {
                    path = path.Replace(this.path, "");
                });
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                var listData = new DeleteObjectsRequest(bucketName, paths);
                client.DeleteObjects(listData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("删除文件异常", e.Message);
                return false;
            }
        }
    }
}
