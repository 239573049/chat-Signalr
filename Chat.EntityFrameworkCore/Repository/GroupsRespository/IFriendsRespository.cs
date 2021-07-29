using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IFriendsRespository : IMasterDbRepositoryBase<Friends>
    {
    }

    public class FriendsRespository : MasterDbRepositoryBase<Friends>, IFriendsRespository
    {
        public FriendsRespository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
