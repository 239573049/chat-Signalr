﻿using AutoMapper;
using Chat.Code.Entities.Groups;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository.GroupsRespository;
using Chat.EntityFrameworkCore;
using System.Threading.Tasks;
using Chat.Application.Dto.GroupsDto;
using System;
using Cx.NetCoreUtils.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Chat.Application.Dto;
using Chat.Uitl.Util;
using System.Collections.Generic;

namespace Chat.Application.AppServices.GroupsService
{
    public interface IGroupDataService
    {
        Task<Guid> CreateGroupData(GroupDataDto group,UserDto userDto);
        Task<GroupDataDto> GetGroup(Guid id);
        Task<bool> UpdateGroupData(GroupDataDto group);
        Task<bool> DeleteGroupData(Guid id, Guid userId);
    }
    public class GroupDataService : BaseService<GroupData>, IGroupDataService
    {
        private readonly IMapper mapper;
        private readonly IGroupMembersRepository groupMembersRepository;
        public GroupDataService(
            IMapper mapper,
            IUnitOfWork<MasterDbContext> unitOfWork,
            IGroupDataRepository groupDataRepository,
            IGroupMembersRepository groupMembersRepository
            ) : base(unitOfWork, groupDataRepository)
        {
            this.mapper = mapper;
            this.groupMembersRepository = groupMembersRepository;
        }

        public async Task<Guid> CreateGroupData(GroupDataDto group, UserDto userDto)
        {
            unitOfWork.BeginTransaction();
            var data = mapper.Map<GroupData>(group);
            var groupMember = new GroupMembers();
            data= await  currentRepository.AddAsync(data);
            data.Receiving = StringUtil.GetString(30);
            data.SelfId = userDto.Id;
            groupMember.GroupDataId = data.Id;
            groupMember.SelfId = userDto.Id;
            await groupMembersRepository.AddAsync(groupMember);
            unitOfWork.CommitTransaction();
            return data.Id; 
        }

        public async Task<bool> DeleteGroupData(Guid id,Guid userId)
        {
            var data =await currentRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (data == null) throw new BusinessLogicException("数据不存在或已被删除");
            if (data.SelfId != userId) throw new BusinessLogicException("没有权限删除群组");
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
