using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface ICreateFriendsRespository : IMasterDbRepositoryBase<CreateFriends>
    {
    }

    public class CreateFriendsRespository : MasterDbRepositoryBase<CreateFriends>, ICreateFriendsRespository
    {
        public CreateFriendsRespository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
