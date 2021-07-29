using Chat.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Core
{
    public interface IRepository<TDbContext, TEntity, TKey> where TEntity : Entity<TKey> where TDbContext : DbContext
    {
        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey id);
        /// <summary>
        /// 根据条件获取集合数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, bool isAsc);
        /// <summary>
        /// 根据条件获取集合数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 根据条件获取集合数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy,
            int pageNo,
            int pageSize,
            bool isAsc);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 返回满足的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 返回满足的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="whereCondition">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="isAsc">true：升序；false：降序</param>
        /// <returns></returns>
        Task<Tuple<IList<TEntity>, int>> GetPageAsync(Expression<Func<TEntity, bool>> whereCondition,
            Expression<Func<TEntity, object>> orderBy,
            int pageNo,
            int pageSize,
            bool isAsc);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entityList">实体</param>
        /// <returns></returns>
        Task AddManyAsync(IList<TEntity> entityList);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        TEntity Update(TEntity entity);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entityList">实体</param>
        /// <returns></returns>
        void UpdateMany(IList<TEntity> entityList);
        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="fields">修改字段名称集合</param>
        /// <returns></returns>
        void PartialUpdate(IList<TEntity> entityList, List<string> fields);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体类id</param>
        Task Delete(TKey id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idList">实体类id</param>
        Task DeleteMany(IList<TKey> idList);
        /// <summary>
        /// 获取个数
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
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
        Task<T> GetFieldQueryFirst<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, T>> scalar);
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
}
