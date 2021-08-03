using Chat.Application.Dto;
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
    public class HubController : ControllerBase
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
            await chatHub.Clients.All.SendAsync("Message" + message.Receiving, message);
            return new OkObjectResult("");
        }

    }
}
