using AutoMapper;
using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using Chat.EntityFrameworkCore;
using System.Threading.Tasks;
using Chat.Application.Dto.GroupsDto;
using System;
using System.Collections.Generic;
using Cx.NetCoreUtils.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.AppServices.GroupsService
{
    public interface IGroupDataService
    {
        Task<Guid> CreateGroupData(GroupDataDto group);
        Task<GroupDataDto> GetGroup(Guid id);
        Task<bool> UpdateGroupData(GroupDataDto group);
        Task<bool> DeleteGroupData(Guid id);
    }
    public class GroupDataService : BaseService<GroupData>, IGroupDataService
    {
        private readonly IMapper mapper;
        public GroupDataService(
            IMapper mapper,
            IUnitOfWork<MasterDbContext> unitOfWork,
            IGroupDataRespository groupDataRespository
            ) : base(unitOfWork, groupDataRespository)
        {
            this.mapper = mapper;
        }

        public async Task<Guid> CreateGroupData(GroupDataDto group)
        {
            var data = mapper.Map<GroupData>(group);
            data= await  currentRepository.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return data.Id; 
        }

        public async Task<bool> DeleteGroupData(Guid id)
        {
            var data =await currentRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            await currentRepository.Delete(id);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task<GroupDataDto> GetGroup(Guid id)
        {
            var data =await currentRepository.FindAll(a=>a.Id==id).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            return mapper.Map<GroupDataDto>(data);
        }

        public async  Task<bool> UpdateGroupData(GroupDataDto group)
        {
            var data =await currentRepository.FindAll(a=>a.Id==group.Id).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            mapper.Map(group,data);
            currentRepository.Update(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }
    }
}
