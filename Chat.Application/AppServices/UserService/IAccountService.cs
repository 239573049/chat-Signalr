using AutoMapper;
using Cx.NetCoreUtils.Exceptions;
using Microsoft.EntityFrameworkCore;
using Chat.Code.DbEnum;
using Chat.Code.Entities.User;
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
    public interface IAccountService
    {
        Task<AccountDto> GetAccount(Guid id);
        Task<AccountDto> GetAccount(string accountNumber);
        Task<Guid> CreateAccount(AccountDto account);
        Task<bool> UpdateAccount(AccountDto account);
        Task<bool> DeleteAccount(Guid id);
        Task<bool> FreezeAccount(List<Guid> ids,DateTime time);
        Task<bool> ThawAccount(List<Guid> ids);
        Task<Tuple<List<AccountDto>,int>> GetAccountList(string name,sbyte status,int pageNo=1,int pageSize=20);
        Task<AccountDto> GetAccountName(string accountNumber);
    }
    public class AccountService : BaseService<Account>, IAccountService
    {
        private readonly IMapper mapper;
        public AccountService(
            IMapper mapper,
            IAccountRepository accountRepository,
            IUnitOfWork<MasterDbContext> unitOfWork
            ) :base(unitOfWork,accountRepository)
        {
            this.mapper = mapper;
        }

        public async Task<Guid> CreateAccount(AccountDto account)
        {
            var data = mapper.Map<Account>(account);
            data.Status = StatusEnum.Start;
            data.Power = PowerEnum.Common;
            data.PassWrod = data.PassWrod.MD5Encrypt();
            data.Freezetime = null;
            data = currentRepository.Add(data);
            await unitOfWork.SaveChangesAsync();
            return data.Id;
        }

        public async  Task<bool> DeleteAccount(Guid id)
        {
            var data =await currentRepository.FindAsync(id);
            if (data == null) throw new BusinessLogicException("用户不存在或者已被删除");
            await currentRepository.Delete(id);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async Task<bool> FreezeAccount(List<Guid> ids, DateTime time)
        {
            if (time < DateTime.Now) throw new BusinessLogicException("封禁时间不能小于当前时间");
            var data =await currentRepository.FindAll(a=>ids.Contains(a.Id)).ToListAsync();
            data.ForEach(a =>{ a.Status = StatusEnum.Freeze; a.Freezetime = time; }) ;
            currentRepository.UpdateMany(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async  Task<AccountDto> GetAccount(Guid id)
        {
            var data =await currentRepository.FindAsync(id);
            if (data == null) throw new BusinessLogicException("用户不存在或者已被删除");
            return mapper.Map<AccountDto>(data);
        }

        public async  Task<AccountDto> GetAccount(string accountNumber)
        {
            var data =await currentRepository.FindAll(a=>a.AccountNumber==accountNumber).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
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
            return mapper.Map<AccountDto>(data);
        }

        public async Task<Tuple<List<AccountDto>,int>> GetAccountList(string name, sbyte status, int pageNo = 1, int pageSize = 20)
        {
            var updates = new List<Account>();
            var data =await currentRepository
                .GetPageAsync(a=>a.Name.ToLower().Contains(name.ToLower()) && status==-1|| a.Status==(StatusEnum)status,a=>a.CreatedTime,pageNo,pageSize,true);
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
            return new Tuple<List<AccountDto>,int>(mapper.Map<List<AccountDto>>(data.Item1),data.Item2);
        }

        public async Task<AccountDto> GetAccountName(string accountNumber)
        {
            var data =await currentRepository.FindAll(a=>a.AccountNumber==accountNumber).OrderBy(a=>a.CreatedTime).FirstOrDefaultAsync();
            if (data == null) throw new BusinessLogicException("不存在此用户");
            return mapper.Map<AccountDto>(data);
        }

        public async Task<bool> ThawAccount(List<Guid> ids)
        {
            var data =await currentRepository.FindAll(a=>ids.Contains(a.Id)).ToListAsync();
            data.ForEach(a => { a.Status = StatusEnum.Start; a.Freezetime = null; });
            currentRepository.UpdateMany(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }

        public async  Task<bool> UpdateAccount(AccountDto account)
        {
            var data =await currentRepository.FindAll(a => a.Id == account.Id).OrderBy(a => a.CreatedTime).FirstOrDefaultAsync();
            if(data==null) throw new BusinessLogicException("用户不存在或者已被删除");
            data.PassWrod = account.PassWrod;
            data.Name = account.Name;
            data.HeadPortrait = account.HeadPortrait;
            currentRepository.Update(data);
            return (await unitOfWork.SaveChangesAsync()) > 0;
        }
    }
}
