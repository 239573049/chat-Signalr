using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Chat.EntityFrameworkCore.Core
{
    public static class DbContextExtension
    {
        /// <summary>
        /// 异步执行带有参数的存储过程方法 获取信息集合以及返回空值处理
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public async static Task<List<TElement>> ExecProcAsync<TElement>(this DbContext db, string sql, SqlParameter[] sqlParams) where TElement : new()
        {

            var connection = db.Database.GetDbConnection();
            using (var cmd = connection.CreateCommand()) {
                await db.Database.OpenConnectionAsync();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams);
                var dr = await cmd.ExecuteReaderAsync();
                var columnSchema = dr.GetColumnSchema();
                var data = new List<TElement>();
                while (await dr.ReadAsync()) {
                    TElement item = new TElement();
                    Type type = item.GetType();
                    foreach (var kv in columnSchema) {
                        var propertyInfo = type.GetProperty(kv.ColumnName);
                        if (kv.ColumnOrdinal.HasValue && propertyInfo != null) {
                            var value = dr.IsDBNull(kv.ColumnOrdinal.Value) ? null : dr.GetValue(kv.ColumnOrdinal.Value);
                            propertyInfo.SetValue(item, value);
                        }
                    }
                    data.Add(item);
                }
                dr.Dispose();
                return data;
            }
        }
    }
}
