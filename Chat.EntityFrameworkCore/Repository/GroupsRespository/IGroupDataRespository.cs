using Chat.Code.Entities.Groups;
using Chat.Code.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IGroupDataRespository : IMasterDbRepositoryBase<GroupData>
    {
    }

    public class GroupDataRespository : MasterDbRepositoryBase<GroupData>, IGroupDataRespository
    {
        public GroupDataRespository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
