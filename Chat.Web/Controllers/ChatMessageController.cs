using AutoMapper;
using Chat.Application.AppServices.GroupsService;
using Chat.Application.Dto.GroupsDto;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 聊天纪录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IChatMessageService chatMessageService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="chatMessageService"></param>
        public ChatMessageController(
            IMapper mapper,
            IChatMessageService chatMessageService
            )
        {
            this.mapper = mapper;
            this.chatMessageService = chatMessageService;
        }
        /// <summary>
        /// 获取聊天纪录
        /// </summary>
        /// <param name="receiving"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ChatMessageDto>> GetChatMessageDataList(string receiving, int pageNo = 1, int pageSize = 50)
        {
            if (pageNo < 1) pageNo = 1;
            if (string.IsNullOrEmpty(receiving)) throw new BusinessLogicException("聊天地址错误");
            return await chatMessageService.GetChatMessageDataList(receiving, pageNo, pageSize);
        }
    }
}
