using Chat.Uitl.Util.HttpUtil;
using Chat.Web.Code.Gadget;
using Chat.Web.Code.Model.ChatVM;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using System;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatHub"></param>
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
            message.Id = Guid.NewGuid();
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
            message.Id = Guid.NewGuid();
            await chatHub.Clients.Group(message.Receiving).SendAsync("ChatData", message);
            return new OkObjectResult("");
        }
        /// <summary>
        /// 获取所有在线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUser()
        {
            return new OkObjectResult(ChatHub.UserData);
        }
        /// <summary>
        /// 发送指定链接地址测试
        /// </summary>
        /// <param name="rediver"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Send(string rediver,[FromBody] MessageVM message)
        {
           await chatHub.Clients.Client(rediver).SendAsync("ChatData",  message);
            return Ok();
        }
    }
}
