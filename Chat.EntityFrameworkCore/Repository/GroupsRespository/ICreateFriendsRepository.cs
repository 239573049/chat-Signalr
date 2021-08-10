using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface ICreateFriendsRepository : IMasterDbRepositoryBase<CreateFriends>
    {
    }

    public class CreateFriendsRepository : MasterDbRepositoryBase<CreateFriends>, ICreateFriendsRepository
    {
        public CreateFriendsRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
