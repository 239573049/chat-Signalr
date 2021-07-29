
using Chat.Core.Base;
using Chat.EntityFrameworkCore;
using Chat.EntityFrameworkCore.Core;
using Chat.EntityFrameworkCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chat.Application
{
    public interface IBaseService<TEntity> where TEntity : Entity, new()
    {
        Task<TEntity> FindAsync(Guid id);

        Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<int> Delete(Guid id);

        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null);

        Task<Guid> InsertAsync(TEntity entity);

        Task<Guid> Update(TEntity entity);

        Task<Tuple<IList<TEntity>, int>> GetPageAsync(int pageSize, int pageIndex, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, object>> orderByLambda, bool isAsc);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 直接获取特定一个或者多个字段的值(m=>m.Code== "02018",m=>m.Name);(m=>m.Code== "02018",m=>new StM { Name1= m.Name, Code=m.Code });
        /// 多个字段(m=>m.Code== "02018",m=>new { m.Name,m.Code });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        Task<IList<T>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar);

        /// <summary>
        /// 分页查询指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="scalar"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Tuple<IList<T>, int>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar,
            Expression<Func<TEntity, object>> orderBy, int pageNo, int pageSize);
    }

    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Entity, new()
    {
        protected IUnitOfWork<MasterDbContext> unitOfWork;
        protected IMasterDbRepositoryBase<TEntity> currentRepository;

        public BaseService(IUnitOfWork<MasterDbContext> unitOfWork, IMasterDbRepositoryBase<TEntity> currentRepository)
        {
            this.unitOfWork = unitOfWork;
            this.currentRepository = currentRepository;
        }

        public async Task<Guid> InsertAsync(TEntity entity)
        {
            await currentRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> Delete(Guid id)
        {
            await currentRepository.Delete(id);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid> Update(TEntity entity)
        {
            currentRepository.Update(entity);
            await unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await currentRepository.CountAsync(predicate);
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            return await currentRepository.FindAsync(id);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await currentRepository.FirstOrDefaultAsync(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return currentRepository.FirstOrDefault(predicate);
        }

        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, List<Expression<Func<TEntity, object>>> includes = null)
        {
            return await currentRepository.GetAsync(predicate, orderBy, includes);
        }

        public async Task<Tuple<IList<TEntity>, int>> GetPageAsync(int pageSize, int pageNo, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, object>> orderByLambda, bool isAsc)
        {
            return await currentRepository.GetPageAsync(whereLambda, orderByLambda, pageNo, pageSize, isAsc);
        }

        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await currentRepository.IsExist(whereLambda);
        }

        public async Task<IList<T>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar)
        {
            return await currentRepository.GetFieldQuery(predicate, scalar);
        }

        public async Task<Tuple<IList<T>, int>> GetFieldQuery<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, T>> scalar, Expression<Func<TEntity, object>> orderBy, int pageNo, int pageSize)
        {
            return await currentRepository.GetFieldQuery(predicate, scalar, orderBy, pageNo, pageSize);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return currentRepository.FindAll(predicate);
        }
    }
}