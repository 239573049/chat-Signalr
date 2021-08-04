using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public class Oss
    {
        public string endpoint;
        public string accessKeyId;
        public string accessKeySecret;
        public string bucketName;
        public string path;
        public Task<string> UploadingFile(string path,Stream stream)
        {
            return Task.Run(() =>
            {
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                client.PutObject(bucketName, path, stream);
                return $"{path}";
            });
        }
        public Task<bool> DeleteFile(string path)
        {
            return Task.Run(() =>
            {
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                client.DeleteObject(bucketName, path);
                return true;
            });
        }
        public bool DeletesFile(List<string> paths)
        {
            try {
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                var listData = new DeleteObjectsRequest(bucketName, paths);
                client.DeleteObjects(listData);
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("删除文件异常", e.Message);
                return false;
            }
        }
    }
}
