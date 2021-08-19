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
        private readonly IRedisUtil redisUtil;
        private readonly IGroupDataRepository groupDataRespository;
        public static ConcurrentDictionary<Guid, string> UserData = new();

        public ChatHub(
            IRedisUtil redisUtil,
            IUserService userService,
            IGroupDataRepository groupDataRespository
            )
        {
            this.redisUtil = redisUtil;
            this.userService = userService;
            this.groupDataRespository = groupDataRespository;
        }
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"] ;
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            if (userDto == null) throw new BusinessLogicException(401,"请先登录");
            UserData.AddOrUpdate(userDto.Id, Context.ConnectionId, (uuId, _) => Context.ConnectionId);
            var group =await groupDataRespository.GetReceiving(userDto.Id);
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
                UserData.Remove(userDto.Id, out string connectionId);
                await userService.UseState(userDto.Id, UseStateEnume.OffLine);
                var group = await groupDataRespository.GetReceiving(userDto.Id);
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
            message.Name = userDto.Name;
            message.HeadPortrait = userDto.HeadPortrait;
            UserData.TryGetValue(Guid.Parse(message.Receiving), out string receiving);
            await Clients.Client(receiving).SendAsync("ChatData",message);
        }

        public async Task SendGroup(MessageVM message)
        {
            message.Key = Guid.NewGuid();

            var token = Context.GetHttpContext().Request.Query["access_token"];
            var userDto = redisUtil.Get<UserDto>(token.ToString());
            message.HeadPortrait = userDto.HeadPortrait;
            message.Name = userDto.Name;
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
