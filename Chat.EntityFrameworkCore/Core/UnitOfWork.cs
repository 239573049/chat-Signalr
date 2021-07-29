using Cx.NetCoreUtils.HttpContext;
using Chat.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Core
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly IPrincipalAccessor _principalAccessor;

        public UnitOfWork(TDbContext dbContext, IPrincipalAccessor principalAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"db context nameof{nameof(dbContext)} is null");
            _principalAccessor = principalAccessor;
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            ApplyChangeConventions();
            try
            {
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception) {
                _dbContext.Database.RollbackTransaction();
                throw;
            }
        }
        
        public void RollbackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public async Task<int> SaveChangesAsync()
        {
            ApplyChangeConventions();
            return await _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            ApplyChangeConventions();
            return _dbContext.SaveChanges();
        }

        protected void ApplyChangeConventions()
        {
            _dbContext.ChangeTracker.DetectChanges();
            var entities = _dbContext.ChangeTracker.Entries().ToList();
            foreach (var entry in entities)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        SetDelete(entry);
                        break;
                    case EntityState.Modified:
                        SetModification(entry.Entity);
                        break;
                    case EntityState.Added:
                        //SetTenant(entry.Entity);
                        SetCreation(entry.Entity);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetCreation(object entityObj)
        {
            if (!(entityObj is IHaveCreation))
            {
                return;
            }

            var entity = (IHaveCreation)entityObj;
            var userId = _principalAccessor.ID;
            if (userId != Guid.Empty)
            {
                entity.CreatedBy = userId;
                entity.CreatedTime = DateTime.Now;
            }
        }

        private void SetCreatedTime(object entityObj)
        {
            if (!(entityObj is IHaveCreatedTime))
            {
                return;
            }
            var entity = (IHaveCreatedTime)entityObj;
            entity.CreatedTime = DateTime.Now;
        }

        private void SetModification(object entityObj)
        {
            if (!(entityObj is IHaveModification))
            {
                return;
            }
            var entity = (IHaveModification)entityObj;
            var userId = _principalAccessor.ID;
            if (userId != Guid.Empty)
            {
                entity.ModifiedTime = DateTime.Now;
                entity.ModifiedBy = userId;
            }
        }

        private void SetModifiedTime(object entityObj)
        {
            if (!(entityObj is IHaveModifiedTime))
            {
                return;
            }

            var entity = (IHaveModifiedTime)entityObj;
            entity.ModifiedTime = DateTime.Now;
        }

        private void SetDelete(EntityEntry entry)
        {
            var entityObj = entry.Entity;
            if (!(entityObj is IHaveDeletion))
            {
                return;
            }

            var entity = (IHaveDeletion)entityObj;
            var userId = _principalAccessor.ID;
            if (userId != Guid.Empty)
            {
                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedTime = DateTime.Now;
                entity.DeletedBy = userId;
            }
        }

        private void SetDeletedTime(object entityObj)
        {
            if (!(entityObj is IHaveDeleteTime))
            {
                return;
            }
            var entity = (IHaveDeleteTime)entityObj;
            entity.DeletedTime = DateTime.Now;
        }
    }
}
