using Chat.Application.AppServices.GroupsService;
using Chat.Application.AppServices.UserService;
using Chat.Application.Dto;
using Chat.Code.DbEnum;
using Chat.MongoDB.Mappings;
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
        private readonly ChatLogConfiguration<MessageVM> chatLogConfiguration;
        private readonly IRedisUtil redisUtil;
        private readonly IGroupMembersService groupMembersService;
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
            IRedisUtil redisUtil,
            IUserService userService,
            IGroupMembersService groupMembersService,
            ChatLogConfiguration<MessageVM> chatLogConfiguration
            )
        {
            this.redisUtil = redisUtil;
            this.userService = userService;
            this.groupMembersService = groupMembersService;
            this.chatLogConfiguration = chatLogConfiguration;
        }
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"] ;
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            if (userDto == null) throw new BusinessLogicException(401,"请先登录");
            if (userDto.Power == PowerEnum.Manage) {
                Admin.AddOrUpdate(userDto.Id, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            }
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
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            if (userDto != null) {
                if(userDto.Power == PowerEnum.Manage) {
                    Admin.Remove(userDto.Id, out string connectionIds);
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

        public async Task Message(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            message.Date = DateTime.Now;
            message.Name = userDto.Name;
            message.HeadPortrait = userDto.HeadPortrait;
            await Clients.Group(message.Receiving).SendAsync("ChatData", message);
        }

        public async Task SendGroup(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            message.HeadPortrait = userDto.HeadPortrait;
            message.Name = userDto.Name;
            message.Date = DateTime.Now;
            await Clients.Group(message.Receiving).SendAsync("ChatData", message);
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
