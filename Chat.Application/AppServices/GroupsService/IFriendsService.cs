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
using Chat.Application.Dto.GroupsDto;
using Cx.NetCoreUtils.Exceptions;

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
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<FriendsDto>> GetFriendsDtos(Guid userId);
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="SelfId"></param>
        /// <param name="FriendId"></param>
        /// <returns></returns>
        Task<bool> DeleteFriends(Guid selfId,Guid friendId);
    }
    public class FriendsService : BaseService<Friends>, IFriendsService 
    {
        private readonly IMapper mapper;
        public FriendsService(
            IMapper mapper,
             IUnitOfWork<MasterDbContext> unitOfWork,
            IFriendsRepository friendsRepository
            ) :base(unitOfWork, friendsRepository)
        {
            this.mapper = mapper;
        }

        public async Task<bool> DeleteFriends(Guid selfId, Guid friendId)
        {
            var data =await currentRepository.FindAll(a=>a.SelfId== selfId || a.SelfId== friendId && a.FriendId== selfId || a.FriendId== friendId).Select(a=>a.Id).ToListAsync();
            if (data.Count == 0) throw new BusinessLogicException("删除失败未找到好友信息");
            await currentRepository.DeleteMany(data);
            return (await unitOfWork.SaveChangesAsync())>0;
        }

        public async Task<List<FriendsDto>> GetFriendsDtos(Guid userId)
        {
            var data =await currentRepository.FindAll(a=>a.SelfId==userId).ToListAsync();
            return mapper.Map<List<FriendsDto>>(data);
        }

        public async Task<bool> GetIsFriends(Guid selfId, Guid friendId)
        {
            var isF =await currentRepository.FindAll(a => a.SelfId == selfId && a.FriendId == friendId).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (isF == null) return false;
            return true;
        }
    }

}
