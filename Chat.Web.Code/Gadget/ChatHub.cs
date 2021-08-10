using Chat.Application.AppServices.UserService;
using Chat.Application.Dto;
using Chat.Code.DbEnum;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using Chat.Uitl.Util;
using Chat.Web.Code.Model.ChatVM;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Code.Gadget
{
    /// <summary>
    /// 推送服务
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IUserService userService;
        private readonly IPrincipalAccessor principalAccessor;
        private readonly IGroupDataRepository groupDataRespository;
        public static ConcurrentDictionary<Guid, string> UserData = new();

        public ChatHub(
            IUserService userService,
            IPrincipalAccessor principalAccessor,
            IGroupDataRepository groupDataRespository
            )
        {
            this.userService = userService;
            this.principalAccessor = principalAccessor;
            this.groupDataRespository = groupDataRespository;
        }
        public override async Task OnConnectedAsync()
        {
            var userId =await principalAccessor.GetUser<UserDto>();
            UserData.AddOrUpdate(userId.Id, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            var group =await groupDataRespository.GetReceiving(userId.Id);
            foreach (var d in group) {
              await Groups.AddToGroupAsync(Context.ConnectionId, d);
            }
            await userService.UseState(userId.Id, UseStateEnume.OnLine);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = await principalAccessor.GetUser<UserDto>();
            if (userId != null) {
                UserData.Remove(userId.Id, out string connectionId);
                await userService.UseState(userId.Id, UseStateEnume.OffLine);
                var group = await groupDataRespository.GetReceiving(userId.Id);
                foreach (var d in group) {
                    await Groups.RemoveFromGroupAsync(connectionId,d);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Message(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            var user = principalAccessor.GetUserDto<UserDto>();
            message.Name = user.Name;
            message.HeadPortrait = user.HeadPortrait;
            UserData.TryGetValue(Guid.Parse(message.Receiving), out string receiving);
            await Clients.Client(receiving).SendAsync("ChatData",message);
        }

        public async Task SendGroup(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            var user = principalAccessor.GetUserDto<UserDto>();
            message.HeadPortrait = user.HeadPortrait;
            message.Name = user.Name;
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
