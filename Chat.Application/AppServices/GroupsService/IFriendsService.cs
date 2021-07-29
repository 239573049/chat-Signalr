using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.AppServices.GroupsService
{
    public interface IFriendsService
    {
        /// <summary>
        /// 是否是好友关系
        /// </summary>
        /// <param name="selfId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<bool> GetIsFriends(Guid selfId,Guid friendId);
    }
    public class FriendsService : BaseService<Friends>, IFriendsService 
    {
        private readonly IMapper mapper;
        public FriendsService(
            IMapper mapper,
             IUnitOfWork<MasterDbContext> unitOfWork,
            IFriendsRespository friendsRespository
            ):base(unitOfWork,friendsRespository)
        {
            this.mapper = mapper;
        }

        public async Task<bool> GetIsFriends(Guid selfId, Guid friendId)
        {
            var isF =await currentRepository.FindAll(a => a.SelfId == selfId && a.FriendId == friendId).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (isF == null) return false;
            return true;
        }
    }

}
