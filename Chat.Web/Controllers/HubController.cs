using Chat.Application.Dto;
using Chat.Uitl.Util.HttpUtil;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Model.ChatVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 推送调试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HubController : UsersController
    {
        private readonly IHubContext<ChatHub> chatHub;
        public HubController(
            IHubContext<ChatHub> chatHub)
        {
            this.chatHub = chatHub;
        }
        /// <summary>
        /// 获取当前人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCount()
        {
            return new OkObjectResult(ChatHub.UserData.Count());
        }
        /// <summary>
        /// 发送调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendId(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            ChatHub.UserData.TryGetValue(Guid.Parse(message.Receiving), out string receiving);
            await chatHub.Clients.Client(receiving).SendAsync("ChatData", message);
            return new OkObjectResult("");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendGroup(MessageVM message)
        {
            message.Key = Guid.NewGuid();
            await chatHub.Clients.Group(message.Receiving).SendAsync("ChatData", message);
            return new OkObjectResult("");
        }
    }
}
