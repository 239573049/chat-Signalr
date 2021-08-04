using AutoMapper;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.EntityFrameworkCore;
using Chat.Code.DbEnum;
using Chat.Code.Entities.Users;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cx.NetCoreUtils.Extensions;
using Chat.Application.Dto;

namespace Chat.Application.AppServices.UserService
{
    public interface IUserService
    {
        Task<UserDto> GetUser(Guid id);
        /// <summary>
        /// 通过账号搜索用户信息
        /// </summary>
        /// <param name="UserNumber"></param>
        /// <returns></returns>
        Task<UserDto> GetUser(string UserNumber);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        Task<Guid> CreateUser(UserDto User);
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        Task<bool> UpdateUser(UserDto User);
        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid id);
        /// <summary>
        /// 封禁账号
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task<bool> FreezeUser(List<Guid> ids,DateTime time);
        /// <summary>
        /// 解封账号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> ThawUser(List<Guid> ids);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Tuple<IList<UserDto>,int>> GetUserList(string name,sbyte status,int pageNo=1,int pageSize=20);
        /// <summary>
        /// 账号在线状态更改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useState"></param>
        /// <returns></returns>
        Task UseState(Guid id, UseStateEnume useState);
        /// <summary>
        /// 管理员编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdatesUsers(UserDto user);

    }
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IMapper mapper;
        public UserService(
            IMapper mapper,
            IUserRepository UserRepository,
            IUnitOfWork<MasterDbContext> unitOfWork
            ) :base(unitOfWork,UserRepository)
        {
            this.mapper = mapper;
        }

        public async Task<Guid> CreateUser(UserDto User)
        {
            var data = mapper.Map<User>(User);
            data.Status = StatusEnum.Start;
            data.UseState = UseStateEnume.OffLine;
            data.Power = PowerEnum.Common;
            data.PassWrod = data.PassWrod.MD5Encrypt();
            data.Freezetime = null;
            data = currentRepository.Add(data);
            await unitOfWork.SaveChangesAsync();
            return data.Id;
        }

        public async  Task<bool> DeleteUser(Guid id)
        {
            var data =await currentRepository.FindAsync(id);
            if (data == null) throw new BusinessLogicException("用户不存在或者已被删除");
            await currentRepository.Delete(id);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task<bool> FreezeUser(List<Guid> ids, DateTime time)
        {
            if (time < DateTime.Now) throw new BusinessLogicException("封禁时间不能小于当前时间");
            var data =await currentRepository.FindAll(a=>ids.Contains(a.Id)).ToListAsync();
            data.ForEach(a =>{ a.Status = StatusEnum.Freeze; a.Freezetime = time; }) ;
            currentRepository.UpdateMany(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async  Task<UserDto> GetUser(Guid id)
        {
            var data =await currentRepository.FindAsync(id);
            if (data == null) throw new BusinessLogicException("用户不存在或者已被删除");
            return mapper.Map<UserDto>(data);
        }

        public async  Task<UserDto> GetUser(string UserNumber)
        {
            var data =await currentRepository.FindAll(a=>a.UserNumber==UserNumber).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("账号不存在");
            if (data.Status == StatusEnum.Freeze)
            {
                if (data.Freezetime < DateTime.Now)
                {
                    data.Status = StatusEnum.Start;
                    data.Freezetime = null;
                    currentRepository.Update(data);
                    await unitOfWork.SaveChangesAsync();
                }
            }
            return mapper.Map<UserDto>(data);
        }

        public async Task<Tuple<IList<UserDto>,int>> GetUserList(string name, sbyte status, int pageNo = 1, int pageSize = 20)
        {
            var updates = new List<User>();
            var data =await currentRepository
                .GetPageAsync(a=>(status==-1?true:a.Status==(StatusEnum)status)&& (string.IsNullOrEmpty(name) ? true : a.Name.ToLower().Contains(name.ToLower())) , a=>a.CreatedTime,pageNo,pageSize,true);
            var now = DateTime.Now;
            foreach (var d in data.Item1)
            {
                if (d.Status == StatusEnum.Freeze)
                {
                    if(d.Freezetime< now)
                    {
                        var update = d;
                        update.Status = StatusEnum.Start;
                        update.Freezetime = null;
                        updates.Add(update);
                    }
                }
            }
            if (updates.Count > 0)
            {
                currentRepository.UpdateMany(updates);
                await unitOfWork.SaveChangesAsync();
            }
            return new Tuple<IList<UserDto>,int>(mapper.Map<IList<UserDto>>(data.Item1),data.Item2);
        }
        public async Task<bool> ThawUser(List<Guid> ids)
        {
            var data =await currentRepository.FindAll(a=>ids.Contains(a.Id)).ToListAsync();
            data.ForEach(a => { a.Status = StatusEnum.Start; a.Freezetime = null; });
            currentRepository.UpdateMany(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdatesUsers(UserDto user)
        {
            var data =await currentRepository.FindAll(a => a.Id == user.Id||user.UserNumber==a.UserNumber).ToListAsync();
            var isUserNumber = data.FirstOrDefault(a => a.UserNumber == user.UserNumber&&a.Id!=user.Id);
            if (isUserNumber != null) throw new BusinessLogicException("用户名已经存在请重新编辑");
            var userData = data.FirstOrDefault(a => a.Id == user.Id);
            if (userData == null) throw new BusinessLogicException("用户不存在或者已被删除");
            userData.Name = user.Name;
            userData.UserNumber = user.UserNumber;
            userData.Status = user.Status;
            if (user.Status == StatusEnum.Freeze) userData.Freezetime = user.Freezetime;
            currentRepository.Update(userData);
            return (await unitOfWork.SaveChangesAsync())>0;
        }

        public async  Task<bool> UpdateUser(UserDto User)
        {
            var data =await currentRepository.FindAll(a => a.Id == User.Id).OrderBy(a => a.CreatedTime).FirstOrDefaultAsync();
            if(data==null) throw new BusinessLogicException("用户不存在或者已被删除");
            data.PassWrod = User.PassWrod;
            data.Name = User.Name;
            data.HeadPortrait = User.HeadPortrait;
            currentRepository.Update(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task UseState(Guid id, UseStateEnume useState)
        {
            var data =await  currentRepository.FindAsync(id);
            if (data == null) throw new BusinessLogicException("用户不存在或者已被删除");
            if (data.UseState == useState) return;
            data.UseState = useState;
            currentRepository.Update(data);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
