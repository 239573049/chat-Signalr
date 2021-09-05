﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Uitl.Util
{
    public class RedisUtil: IRedisUtil
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return RedisHelper.Get(key);
        }

        public T Get<T>(string key) where T : new()
        {
            return RedisHelper.Get<T>(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expiresSec"></param>
        public void Set(string key, object t, DateTime? expiresSec=null)
        {
            RedisHelper.Set(key, t);
            if (expiresSec != null)SetDate(key, (DateTime)expiresSec);
        }
        /// <summary>
        /// 异步获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            return await RedisHelper.GetAsync(key);
        }
        /// <summary>
        /// 获取值泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await RedisHelper.GetAsync<T>(key);
        }
        /// <summary>
        /// 修改或新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expiresSec"></param>
        /// <returns></returns>
        public async Task SetAsync(string key, object t, DateTime? expiresSec = null)
        {
            await RedisHelper.SetAsync(key, t);
            if(expiresSec!=null) await SetDateAsync(key,(DateTime)expiresSec);
        }
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool SetDate(string key,DateTime date)
        {
            return RedisHelper.PExpireAt(key,date);
        }
        /// <summary>
        /// 设置超时时间异步
        /// </summary>
        /// <param name="key"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public Task<bool> SetDateAsync(string key, DateTime date)
        {
            return RedisHelper.PExpireAtAsync(key, date);
        }
        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key)
        {
            return (await RedisHelper.DelAsync(key))>0;
        }
        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return RedisHelper.Del(key) > 0;
        }
        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool HSetNx(string key, string field, object value)
        {
            return RedisHelper.HSetNx(key,field,value);
        }

        public async Task<string[]> GetReceivings(Guid id)
        {
            var data =await RedisHelper.GetAsync(id.ToString());
            if (!string.IsNullOrEmpty(data)) {
                var receivings = data.Split(",");
                return receivings;
            }
            return Array.Empty<string>();
        }

        public async Task<bool> SetReceivings(Guid id, string receiving)
        {
            var data=await RedisHelper.GetAsync(id.ToString());
            if (!string.IsNullOrEmpty(data)) {
                await RedisHelper.SetAsync(id.ToString(), data + "," + receiving);
                return true;
            }
            else {
                await RedisHelper.SetAsync(id.ToString(), receiving);
                return true;
            }

        }

        public async Task<bool> DeleteReceivings(Guid id, string receiving)
        {
            var data = await RedisHelper.GetAsync(id.ToString());
            if (!string.IsNullOrEmpty(data)) {
                var receivings = data.Split(",");
                receivings = receivings.Where(a => a != receiving).ToArray();
                if (receivings.Length == 0) {
                    await RedisHelper.DelAsync(id.ToString());
                    return true;
                }
                await RedisHelper.SetAsync(id.ToString(), string.Join(',', receivings));
                return true;
            }
            return true;
        }
    }
}
