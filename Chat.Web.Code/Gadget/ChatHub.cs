using Chat.Application.AppServices.UserService;
using Chat.Application.Dto;
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
        private readonly IPrincipalAccessor principalAccessor;
        public static ConcurrentDictionary<Guid, string> UserData = new ConcurrentDictionary<Guid, string>();
        public ChatHub(
            IUserService userService,
            IPrincipalAccessor principalAccessor
            )
        {
            this.userService = userService;
            this.principalAccessor = principalAccessor;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = principalAccessor.GetId();
            UserData.AddOrUpdate(userId, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            await userService.UseState(userId, UseStateEnume.OnLine);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = principalAccessor.GetId();
            if (userId != Guid.Empty) {
                UserData.Remove(userId, out string connectionId);
                await userService.UseState(userId, UseStateEnume.OffLine);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task Message(MessageVM message)
        {
            await Clients.All.SendAsync("Message" + message.Receiving,message);
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
