using Chat.Code.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IGroupMembersRespository : IMasterDbRepositoryBase<GroupMembers>
    {
    }

    public class GroupMembersRespository : MasterDbRepositoryBase<GroupMembers>, IGroupMembersRespository
    {
        public GroupMembersRespository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
