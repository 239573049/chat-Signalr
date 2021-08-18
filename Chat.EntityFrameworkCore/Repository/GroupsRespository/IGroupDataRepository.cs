﻿using Chat.Code.Entities.Groups;
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
        /// 获取链接id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetReceiving(Guid userId);
    }

    public class GroupDataRepository : MasterDbRepositoryBase<GroupData>, IGroupDataRepository
    {
        public GroupDataRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }

        public async  Task<List<string>> GetReceiving(Guid userId)
        {
           return await DbSet.Where(a => a.SelfId == userId).Select(a => a.Receiving).ToListAsync();
        }
    }
}