
using Chat.MongoDB.Mappings;
using Chat.Web.Code.Model.ChatVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatLogController : ControllerBase
    {
        private readonly ChatLogConfiguration<MessageVM> chatLogConfiguration;
        public ChatLogController(
            ChatLogConfiguration<MessageVM> chatLogConfiguration
            )
        {
            this.chatLogConfiguration = chatLogConfiguration;
        }
        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MessageVM> GetChatData(string receiving)
        {
            return chatLogConfiguration.collection.Find(a => a.Receiving== receiving).ToList().OrderBy(a=>a.Date).ToList(); 
        }

    }
}
