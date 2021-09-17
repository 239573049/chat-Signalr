using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.AppServices.UserService;
using Chat.Application.Dto;
using Chat.Application.Dto.GroupsDto;
using Chat.Code.DbEnum;
using Chat.Uitl.Util;
using Chat.Web.Code.Model.ChatVM;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Chat.Web.Code.Gadget
{
    /// <summary>
    /// 推送服务
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IUserService userService;
        private readonly object _lock = new();
        private readonly IRedisUtil redisUtil;
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IMapper mapper;
        private readonly IGroupMembersService groupMembersService;
        private readonly IChatMessageService chatMessageService;
        public static ConcurrentDictionary<Guid, string> UserData { get; set; } = new();
        public static ConcurrentDictionary<Guid, string> Admin { get; set; } = new();
        public static string GetUserData(Guid key)
        {
            var receiving = string.Empty;
            try {
                UserData.TryGetValue(key,out receiving);
            }
            catch (ArgumentNullException) {
                return receiving;
            }
            return receiving;
        }
        public static List<string> GetUserData(List<Guid> key)
        {
            var receivings =new List<string>();
            foreach (var d in key) {
                try {
                    UserData.TryGetValue(d, out string receiving);
                    receivings.Add(receiving);
                }
                catch (ArgumentNullException) {
                }
            }
            return receivings;
        }
        public ChatHub(
            IMapper mapper,
            IRedisUtil redisUtil,
            IUserService userService,
            IHubContext<ChatHub> chatHub,
            IChatMessageService chatMessageService,
            IGroupMembersService groupMembersService
            )
        {
            this.mapper = mapper;
            this.chatHub = chatHub;
            this.redisUtil = redisUtil;
            this.userService = userService;
            this.chatMessageService = chatMessageService;
            this.groupMembersService = groupMembersService;
        }
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"] ;
            var isPower = Context.GetHttpContext().Request.Query["isPower"].ToString()=="null" || string.IsNullOrEmpty(Context.GetHttpContext().Request.Query["isPower"].ToString()) ? "" : Context.GetHttpContext().Request.Query["isPower"].ToString();
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            if (userDto == null) throw new BusinessLogicException(401,"请先登录");
            if (userDto.Power == PowerEnum.Manage) {
                if (!string.IsNullOrEmpty(isPower) && Convert.ToBoolean(isPower)) {
                    await redisUtil.SAdd("admin", Context.ConnectionId);
                    if (!TimedTask.IsStatus) {
                        lock (_lock) TimedTask.IsStatus = true;
                        if (TimedTask.HubContext == null) TimedTask.HubContext = chatHub;
                        if (TimedTask.redisUtil == null) TimedTask.redisUtil = redisUtil;
                        TimedTask.Tasks();
                    }
                }
            }
            await redisUtil.SAdd("connection"+userDto.Id, Context.ConnectionId);
            UserData.AddOrUpdate(userDto.Id, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            var group =await groupMembersService.GetReceiving(userDto.Id);
            foreach (var d in group) {
              await Groups.AddToGroupAsync(Context.ConnectionId, d);
            }
            await userService.UseState(userDto.Id, UseStateEnume.OnLine);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var isPower = Context.GetHttpContext().Request.Query["isPower"].ToString() == "null" || string.IsNullOrEmpty(Context.GetHttpContext().Request.Query["isPower"].ToString()) ? "" : Context.GetHttpContext().Request.Query["isPower"].ToString();
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            if (userDto != null) {
                await redisUtil.SRemAsync("connection" + userDto.Id, Context.ConnectionId);
                if (userDto.Power == PowerEnum.Manage) {
                    if (!string.IsNullOrEmpty(isPower) && Convert.ToBoolean(isPower)) {
                        await redisUtil.SRemAsync("admin", Context.ConnectionId);
                        if(await redisUtil.IsExist("admin")) {
                            if (TimedTask.IsStatus) {
                                lock (_lock) TimedTask.IsStatus = false;
                                if (TimedTask.HubContext != null) TimedTask.HubContext = null;
                                if (TimedTask.redisUtil != null) TimedTask.redisUtil = null;
                            }
                        }
                    }
                }
                UserData.Remove(userDto.Id, out string connectionId);
                await userService.UseState(userDto.Id, UseStateEnume.OffLine);
                var group = await groupMembersService.GetReceiving(userDto.Id);
                foreach (var d in group) {
                    await Groups.RemoveFromGroupAsync(connectionId,d);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Message(ChatMessageDto message)
        {
            message.Id = Guid.NewGuid();
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            message.Date = DateTime.Now;
            message.Name = userDto.Name;
            message.HeadPortrait = userDto.HeadPortrait;
            await Clients.Group(message.Receiving).SendAsync("ChatData", message);
            await chatMessageService.CreateChatMessage(mapper.Map<ChatMessageDto>(message));
        }

        public async Task SendGroup(ChatMessageDto message)
        {
            message.Id = Guid.NewGuid();
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            message.HeadPortrait = userDto.HeadPortrait;
            message.Name = userDto.Name;
            message.Date = DateTime.Now;
            await Clients.Group(message.Receiving).SendAsync("ChatData", message);
            await chatMessageService.CreateChatMessage(mapper.Map<ChatMessageDto>(message));
        }
        /// <summary>
        /// 系统推送
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public async Task SystemMessage(SystemMassageVM system)
        {
            await Clients.All.SendAsync("SystemMessage",system);
        }
    }
}
