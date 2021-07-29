using Chat.Web.Code.Model.ChatVM;
using Cx.NetCoreUtils.Exceptions;
using Cx.NetCoreUtils.HttpContext;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Web.Code.Gadget
{
    [Authorization]
    public class ChatHub : Hub
    {
        private readonly IPrincipalAccessor principalAccessor;
        public static ConcurrentDictionary<Guid, string> UserData = new ConcurrentDictionary<Guid, string>();
        public ChatHub(
            IPrincipalAccessor principalAccessor
            )
        {
            this.principalAccessor = principalAccessor;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = principalAccessor.ID;
            if (userId == Guid.Empty) throw new BusinessLogicException(401, "未获取到登录用户的相关信息请先登录");
            UserData.AddOrUpdate(userId, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = principalAccessor.ID;
            if (userId != Guid.Empty) {
                UserData.Remove(userId, out string connectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task Message(MessageVM message)
        {
            await Clients.All.SendAsync("Message" + message.Receiving,message);
        }
        public async Task SystemMessage(SystemMassageVM system)
        {
            await Clients.All.SendAsync("SystemMessage" + system.Receiving,system);
        }
    }
}
