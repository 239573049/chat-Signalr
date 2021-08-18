using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IFriendsRepository : IMasterDbRepositoryBase<Friends>
    {
    }

    public class FriendsRepository : MasterDbRepositoryBase<Friends>, IFriendsRepository
    {
        public FriendsRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
