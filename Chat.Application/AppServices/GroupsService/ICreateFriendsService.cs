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
using Chat.Uitl.Util;
using Chat.Application.Dto;

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
        Task<Tuple<IList<CreateFriendsDto>, int>> GetCreateFriendsDtos(Guid userId, int pageNo = 1, int pageSize = 20);
        Task<Tuple<CreateFriendsDto,string>> ChangeCreateFriends(Guid id,CreateFriendsEnum create);
    }
    public class CreateFriendsService:BaseService<CreateFriends>,ICreateFriendsService
    {
        private readonly IMapper mapper;
        private readonly IFriendsService friendsService;
        private readonly IFriendsRepository friendsRepository;
        public CreateFriendsService(
            IMapper mapper,
            IFriendsService friendsService,
            IFriendsRepository friendsRepository,
            IUnitOfWork<MasterDbContext> unitOfWork,
            ICreateFriendsRepository createFriendsRepository
            ) :base(unitOfWork, createFriendsRepository)
        {
            this.mapper = mapper;
            this.friendsService = friendsService;
            this.friendsRepository = friendsRepository;
        }

        public async Task<Tuple<CreateFriendsDto, string>> ChangeCreateFriends(Guid id, CreateFriendsEnum create)
        {
            if (create == CreateFriendsEnum.Applying) throw new BusinessLogicException("错误状态");
            var data =await currentRepository.FindAll(a=>a.Id==id)
                .Include(a=>a.BeInvited)
                .Include(a=>a.Initiator)
                .OrderBy(a=>a.CreatedTime)
                .FirstOrDefaultAsync();
            data.CreateFriendsEnum = create;
            var receiving = StringUtil.GetString(30);
            if (create == CreateFriendsEnum.Consent) {
                var friends = new List<Friends>
                {
                    new Friends { FriendId = data.BeInvitedId, SelfId = data.InitiatorId, Receiving =receiving },
                    new Friends { SelfId = data.BeInvitedId, FriendId = data.InitiatorId, Receiving =receiving }
                };
                await friendsRepository.AddManyAsync(friends);
            }
            currentRepository.Update(data);
            await unitOfWork.SaveChangesAsync();
            return new Tuple<CreateFriendsDto, string>(mapper.Map<CreateFriendsDto>(data), receiving);
        }

        public async Task<Guid> CreateCreateFriends(CreateFriendsDto create)
        {
            var isF =await friendsService.GetIsFriends(create.InitiatorId, create.BeInvitedId);
            if (isF) throw new BusinessLogicException("已经是好友无法重复添加");
            var isData = await currentRepository.IsExist(a => a.InitiatorId == create.InitiatorId && a.BeInvitedId == create.BeInvitedId&&a.CreateFriendsEnum==CreateFriendsEnum.Applying);
            if (isData) throw new BusinessLogicException("已申请添加好友还未通过");
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

        public async Task<Tuple<IList<CreateFriendsDto>,int>> GetCreateFriendsDtos(Guid userId,int pageNo=1,int pageSize=20)
        {
            var data =await currentRepository
                .GetFieldQuery(a => a.InitiatorId == userId || a.BeInvitedId == userId,a=>new CreateFriendsDto {Id=a.Id,BeInvitedId=a.BeInvitedId,BeInvited=mapper.Map<UsersDto>(a.BeInvited),CreateFriendsCode=EnumExtensionUtil.GetEnumStringVal(a.CreateFriendsEnum),CreateFriendsEnum=a.CreateFriendsEnum,InitiatorId=a.InitiatorId,Remark=a.Remark,Initiator=mapper.Map<UsersDto>(a.Initiator) },a=>a.CreatedTime,pageNo,pageSize);
            return data;
        }
    }
}
