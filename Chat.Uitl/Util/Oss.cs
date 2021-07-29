using Aliyun.OSS;
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
                return $"{this.path}/{path}";
            });
        }

    }
}
