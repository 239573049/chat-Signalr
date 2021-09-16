using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore.Mappings.GroupsConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IChatMessageRespository : IMasterDbRepositoryBase<ChatMessage>
    {
    }

    public class ChatMessageRepository : MasterDbRepositoryBase<ChatMessage>, IChatMessageRespository
    {
        public ChatMessageRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
