using Chat.Uitl.Util;
using Cx.NetCoreUtils.Common;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Chat.MongoDB
{
    public class BaseService<T> 
    {
        public readonly IMongoCollection<T> collection;   //数据表操作对象
        public BaseService(string name)
        {
            var client = new MongoClient(AppSettingsUtil.App("ConnectionString:MongoDB"));
            var database = client.GetDatabase(AppSettingsUtil.App("ConnectionString:MongoDBData"));
            collection = database.GetCollection<T>(name);
        }
        public List<T> Get()
        {
            return collection.Find<T>(a=>true).ToList();
        }
        public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await collection.Find<T>(predicate).ToListAsync();
        }
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            collection.DeleteOne(predicate);
        }
        public  void Update(Expression<Func<T, bool>> predicate,T t)
        {
            collection.ReplaceOne(predicate,t);
        }

        public void Create(T t) {
            collection.InsertOne(t);

        }
    }
}
