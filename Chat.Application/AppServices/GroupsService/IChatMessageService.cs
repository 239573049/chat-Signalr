using AutoMapper;
using Chat.Application;
using Chat.Application.Dto.GroupsDto;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Mappings.GroupsConfiguration;
using Chat.EntityFrameworkCore.Repository;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.AppServices.GroupsService
{
    public interface IChatMessageService
    {
        Task<Guid> CreateChatMessage(ChatMessageDto chat);
        Task<List<ChatMessageDto>> GetChatMessageDataList(string receiving,int pageNo=1,int pageSize=20);
    }
    public class ChatMessageService : BaseService<ChatMessage>, IChatMessageService
    {
        private readonly IMapper mapper;
        public ChatMessageService(
            IMapper mapper,
            IUnitOfWork<MasterDbContext> unitOfWork,
            IChatMessageRespository chatMessageRespository
            ) : base(unitOfWork, chatMessageRespository)
        {
            this.mapper = mapper;
        }

        public async Task<Guid> CreateChatMessage(ChatMessageDto chat)
        {
            var chats=mapper.Map<ChatMessage>(chat);
            chats =await currentRepository.AddAsync(chats);
            await unitOfWork.SaveChangesAsync();
            return chats.Id;
        }

        public async Task<List<ChatMessageDto>> GetChatMessageDataList(string receiving, int pageNo = 1, int pageSize = 20)
        {
            var date = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));
            var data =await currentRepository
                .FindAll(a=>a.Receiving==receiving&&a.Date> date)
                .AsNoTracking()
                .OrderByDescending(a=>a.Date)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return mapper.Map<List<ChatMessageDto>>(data);
        }
    }
}
