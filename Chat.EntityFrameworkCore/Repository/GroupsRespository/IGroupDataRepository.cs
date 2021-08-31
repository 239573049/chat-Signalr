using Chat.Code.Entities.Groups;
using Chat.Code.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.GroupsRespository
{
    public interface IGroupDataRepository : IMasterDbRepositoryBase<GroupData>
    {
        /// <summary>
        /// 是否存在群
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsExist(Guid id);
    }

    public class GroupDataRepository : MasterDbRepositoryBase<GroupData>, IGroupDataRepository
    {
        public GroupDataRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
        public bool IsExist(Guid id) =>
            DbSet.Any(a => a.Id == id);
    }
}
