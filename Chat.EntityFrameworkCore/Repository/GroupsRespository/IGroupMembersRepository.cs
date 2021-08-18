using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IGroupMembersRepository : IMasterDbRepositoryBase<GroupMembers>
    {
    }

    public class GroupMembersRepository : MasterDbRepositoryBase<GroupMembers>, IGroupMembersRepository
    {
        public GroupMembersRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
