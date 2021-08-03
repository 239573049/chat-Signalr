using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Application.Dto.GroupsDto;
using AutoMapper;
using Cx.NetCoreUtils.Exceptions;
using Chat.Code.DbEnum;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.AppServices.GroupsService
{
    public interface ICreateFriendsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateFriendsDto> GetCreateFriends(Guid id);
        Task<Guid> CreateCreateFriends(CreateFriendsDto create);
        Task<Tuple<List<CreateFriendsDto>, int>> GetCreateFriendsDtos(Guid userId, int pageNo = 1, int pageSize = 20);
        Task<bool> ChangeCreateFriends(Guid id,CreateFriendsEnum create);
    }
    public class CreateFriendsService:BaseService<CreateFriends>,ICreateFriendsService
    {
        private readonly IMapper mapper;
        private readonly IFriendsService friendsService;
        public CreateFriendsService(
            IMapper mapper,
            IFriendsService friendsService,
            IUnitOfWork<MasterDbContext> unitOfWork,
            ICreateFriendsRespository createFriendsRespository
            ):base(unitOfWork,createFriendsRespository)
        {
            this.mapper = mapper;
            this.friendsService = friendsService;
        }

        public async Task<bool> ChangeCreateFriends(Guid id, CreateFriendsEnum create)
        {
            if (create == CreateFriendsEnum.Applying) throw new BusinessLogicException("错误状态");
            var data =await currentRepository.FindAsync(id);
            data.CreateFriendsEnum = create;
            await Update(data);
            return true;
        }

        public async Task<Guid> CreateCreateFriends(CreateFriendsDto create)
        {
            var isF =await friendsService.GetIsFriends(create.InitiatorId, create.BeInvitedId);
            if (isF) throw new BusinessLogicException("已经是好友无法重复添加");
            var data = mapper.Map<CreateFriends>(create);
            data.CreateFriendsEnum = CreateFriendsEnum.Applying;
            data=await currentRepository.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return data.Id;
        }

        public async Task<CreateFriendsDto> GetCreateFriends(Guid id)
        {
            var friends =await currentRepository.FindAsync(id);
            return mapper.Map<CreateFriendsDto>(friends);
        }

        public async Task<Tuple<List<CreateFriendsDto>,int>> GetCreateFriendsDtos(Guid userId,int pageNo=1,int pageSize=20)
        {
            var data =await currentRepository.GetPageAsync(a => a.InitiatorId == userId || a.BeInvitedId == userId,a=>a.CreatedTime,pageNo,pageSize,true);
            return mapper.Map<Tuple<List<CreateFriendsDto>, int>>(data);
        }
    }
}
