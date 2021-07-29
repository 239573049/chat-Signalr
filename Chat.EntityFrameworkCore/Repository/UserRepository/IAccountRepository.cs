using Chat.Code.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Repository.UserRepository
{

    public interface IAccountRepository : IMasterDbRepositoryBase<Account>
    {
    }

    public class AccountRepository : MasterDbRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(MasterDbContext masterDbContext) : base(masterDbContext) { }
    }
}
