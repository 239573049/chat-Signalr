using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Core
{
    public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        void BeginTransaction();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
