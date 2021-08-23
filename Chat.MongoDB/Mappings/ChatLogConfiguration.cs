
using Microsoft.Extensions.Configuration;

namespace Chat.MongoDB.Mappings
{
    public class ChatLogConfiguration<T>:BaseService<T>
    {
        public ChatLogConfiguration(
            IConfiguration config) : base(config,nameof(T))
        {

        }
    }
}
