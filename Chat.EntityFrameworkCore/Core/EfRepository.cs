using Chat.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Core
{
    public abstract class EfRepository<TDbContext, TEntity, TKey> : IRepository<TDbContext, TEntity, TKey>
        where TEntity : Entity<TKey>
        where TDbContext : DbContext
    {
        protected readonly TDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public EfRepository(TDbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }
        public virtual TEntity Add(TEntity entity)
        {
            var result = DbSet.Add(entity);
            return result.Entity;
        }
        public virtual async Task AddManyAsync(IList<TEntity> entityList)
        {
            await DbSet.AddRangeAsync(entityList);
        }
        public virtual TEntity Update(TEntity entity)
        {
            //if (entity == null) throw new BusinessLogicException("update object couldn't be null");
            AttachIfNot(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
        public virtual void UpdateMany(IList<TEntity> entityList)
        {
            //if (entityList == null) throw new BusinessLogicException("update object couldn't be null");
            AttachIfNot(entityList);
            Context.UpdateRange(entityList);
        }
        public virtual async Task Delete(TKey id)
        {
            TEntity entity = await DbSet.FindAsync(id);
            if (entity == null) return; // throw new BusinessLogicException("the deleted object does not exist");
            AttachIfNot(entity);
            DbSet.Remove(entity);
        }
        public virtual async Task DeleteMany(IList<TKey> idList)
        {
            var entityList = await DbSet.Where(a => idList.Contains(a.Id)).ToListAsync();
            if (entityList == null) return;// throw new BusinessLogicException("the deleted objects do not exist");
            AttachIfNot(entityList);
            DbSet.RemoveRange(entityList);
        }
        public void PartialUpdate(IList<TEntity> entityList, List<string> fields)
        {
            var type = typeof(TEntity);
            AttachIfNot(entityList);
            foreach (var entity in entityList)
            {
                Context.Entry(entity).State = EntityState.Modified;
                foreach (var property in type.GetProperties().Where(a => a.PropertyType.FullName != null && !a.PropertyType.FullName.Contains("Chat.Core.Entities")))
                {
                    var fieldName = property.Name;
                    Context.Entry(entity).Property(fieldName).IsModified = fields.Contains(fieldName);
                }
            }
        }
        protected virtual void AttachIfNot(TEntity entity)
        {
            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
        }
        protected virtual void AttachIfNot(IList<TEntity> entityList)
        {
            foreach (var entity in entityList.Where(entity => this.Context.Entry(entity).State == EntityState.Detached))
            {
                DbSet.Attach(entity);
            }
        }
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }

        public virtual async Task<TEntity> FindAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, bool isAsc)
        {
            return isAsc ? await DbSet.AsNoTracking().Where(predicate).OrderBy(orderBy).ToListAsync() : await DbSet.AsNoTracking().Where(predicate).OrderByDescending(orderBy).ToListAsync();
        }
        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate);
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy,
            int pageNo,
            int pageSize,
            bool isAsc)
        {
            return isAsc ? DbSet.AsNoTracking().Where(predicate).OrderBy(orderBy) : DbSet.AsNoTracking().Where(predicate).OrderByDescending(orderBy);
        }
        public virtual async Task<Tuple<IList<TEntity>, int>> GetPageAsync(Expression<Func<TEntity, bool>> whereCondition,
            Expression<Func<TEntity, object>> orderBy,
            int pageNo,
            int pageSize,
            bool isAsc)
        {
            var total = await DbSet.Where(whereCondition).CountAsync();
            var entities = isAsc ?
                await DbSet.AsNoTracking().Where(whereCondition)
                    .OrderBy(orderBy)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync() :
                await DbSet.AsNoTracking().Where(whereCondition)
                    .OrderByDescending(orderBy)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            return new Tuple<IList<TEntity>, int>(entities, total);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await DbSet.Where(whereLambda).AnyAsync();
        }
        public virtual async Task<IList<T>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar)
        {
            if (predicate == null)
            {
                return await DbSet.AsNoTracking().Select(scalar).ToListAsync();
            }
            return await DbSet.AsNoTracking().Where(predicate).Select(scalar).ToListAsync();
        }
        public virtual async Task<T> GetFieldQueryFirst<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, T>> scalar)
        {
            if (predicate == null)
            {
                return await DbSet.AsNoTracking().OrderByDescending(orderBy).Select(scalar).FirstOrDefaultAsync();
            }
            return await DbSet.AsNoTracking().OrderByDescending(orderBy).Where(predicate).Select(scalar).FirstOrDefaultAsync();
        }
        public virtual async Task<Tuple<IList<T>, int>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar, Expression<Func<TEntity, object>> orderBy, int pageNo, int pageSize)
        {
            List<T> entities;
            if (predicate == null)
            {
                entities = await DbSet.AsNoTracking().OrderByDescending(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).Select(scalar).ToListAsync();
                return new Tuple<IList<T>, int>(entities, await DbSet.AsNoTracking().CountAsync());
            }
            entities = await DbSet.AsNoTracking().Where(predicate).OrderByDescending(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).Select(scalar).ToListAsync();
            return new Tuple<IList<T>, int>(entities, await DbSet.AsNoTracking().Where(predicate).CountAsync());
        }
    }
}
