
using Microsoft.Extensions.Configuration;

namespace Chat.MongoDB.Mappings
{
    public class ChatLogConfiguration<T>:BaseService<T>
    {
        public ChatLogConfiguration() : base(nameof(T))
        {

        }
    }
}
