using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Code.DbEnum;
using Chat.Web.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Web.Code.Model;
using Chat.Application.Dto.GroupsDto;
using Chat.Uitl.Util;
using Chat.Application.Dto;
using Chat.Web.Code.Gadget;
using Microsoft.AspNetCore.SignalR;
using Chat.Web.Code.Model.SystemVM;
using Cx.NetCoreUtils.Exceptions;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 添加申请好友
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorization]
    public class CreateFriendsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRedisUtil redisUtil;
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly ICreateFriendsService createFriendsService;
        public CreateFriendsController(
            IMapper mapper,
            IRedisUtil redisUtil,
            IHubContext<ChatHub> chatHub,
            IPrincipalAccessor principalAccessor,
            ICreateFriendsService createFriendsService
            )
        {
            this.mapper = mapper;
            this.chatHub = chatHub;
            this.redisUtil = redisUtil;
            this.principalAccessor = principalAccessor;
            this.createFriendsService = createFriendsService;
        }
        /// <summary>
        /// 通过或拒绝好友申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> ChangeCreateFriends(Guid id, CreateFriendsEnum create)
        {
            var userId = await createFriendsService.ChangeCreateFriends(id, create);
            var initiatorIds = await redisUtil.SMembersAsync<string>("connection" + userId.Item1.InitiatorId);
            var beInvitedIds = await redisUtil.SMembersAsync<string>("connection" + userId.Item1.BeInvitedId);
            for (int i = 0; i < initiatorIds.Length; i++) {
                await chatHub.Clients.Clients(initiatorIds[i])
                                  .SendAsync("SystemMessage",
                                  new SystemPushVM { Key = Guid.NewGuid(), Data = create == CreateFriendsEnum.Consent ? "您的好友已经同意您的好友申请了，快去愉快的聊天吧！" : "您的好友申请已经被拒绝！", Name = "好友申请回复", IsRead = false, SystemMarking = SysytemMarkingEnum.FriendRequestStatus });
               if(create == CreateFriendsEnum.Consent)await chatHub.Groups.AddToGroupAsync(initiatorIds[i], userId.Item2);
            }
            for (int i = 0; i < beInvitedIds.Length; i++) {
                await chatHub.Groups.AddToGroupAsync(beInvitedIds[i], userId.Item2);
            }
            return true;
        }
        /// <summary>
        /// 获取好友申请列表信息
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PeddleDataResponse<IList<CreateFriendsDto>>> GetCreateFriendsDtos(int pageNo = 1, int pageSize = 20)
        {
            var userDto = await principalAccessor.GetUser<UserDto>();
            var data = await createFriendsService.GetCreateFriendsDtos(userDto.Id, pageNo, pageSize);
            return new PeddleDataResponse<IList<CreateFriendsDto>>(data.Item1, data.Item2);
        }
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CreateCreateFriends(CreateFriendsDto create)
        {
            var userDto = await principalAccessor.GetUser<UserDto>();
            if (userDto.Id == create.BeInvitedId) throw new BusinessLogicException("无法对自己发送添加好友请求");
            create.InitiatorId = userDto.Id;
            var data = await createFriendsService.CreateCreateFriends(create);
            var ids = await redisUtil.SMembersAsync<string>("connection" + create.BeInvitedId);
            for (int i = 0; i < ids.Length; i++) {
                await chatHub.Clients.Clients(ids[i])
                 .SendAsync("SystemMessage", new SystemPushVM { Key = Guid.NewGuid(), Data = "您有一条新的好友申请请及时查看！", Name = "好友申请", IsRead = false, SystemMarking = SysytemMarkingEnum.FriendRequest });
            }
            return data != Guid.Empty;
        }
    }
}
