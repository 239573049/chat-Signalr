using AutoMapper;
using Chat.Application.Dto.GroupsDto;
using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.AppServices.GroupsService
{
    public interface IGroupMembersService
    {
        Task<GroupMembersDto> GetGroupMembers(Guid id);
        Task<List<GroupMembersDto>> GetGroupMembersList(Guid userId);
        Task<Guid> CreateGroupMembers(GroupMembersDto group);
        Task<bool> UpdateGroupMembers(GroupMembersDto group);
        Task<bool> DeleteGroupMembers(Guid id);
    }
    public class GroupMembersService : BaseService<GroupMembers>, IGroupMembersService
    {
        private readonly IMapper mapper;
        public GroupMembersService(
            IMapper mapper,
            IUnitOfWork<MasterDbContext> unitOfWork,
            IGroupMembersRespository groupMembersRespository
            ):base(unitOfWork,groupMembersRespository)
        {
            this.mapper = mapper;
        }

        public async Task<Guid> CreateGroupMembers(GroupMembersDto group)
        {
            var data = mapper.Map<GroupMembers>(group);
            data= await currentRepository.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return data.Id;
        }

        public async Task<bool> DeleteGroupMembers(Guid id)
        {
            var data =await currentRepository.FindAll(a=>a.Id==id).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            return(await Delete(id))>0;
        }

        public async Task<GroupMembersDto> GetGroupMembers(Guid id)
        {
            var data =await FindAsync(id);
            return mapper.Map<GroupMembersDto>(data);
        }

        public async Task<List<GroupMembersDto>> GetGroupMembersList(Guid userId)
        {
            var data =await currentRepository.FindAll(a => a.SelfId == userId).ToListAsync();
            return mapper.Map<List<GroupMembersDto>>(data);
        }

        public async Task<bool> UpdateGroupMembers(GroupMembersDto group)
        {
            var data =await    currentRepository.FindAll(a => a.Id == group.Id).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            mapper.Map(group, data);
            currentRepository.Update(data);
            return(await unitOfWork.SaveChangesAsync())>0;
        }
    }
}
