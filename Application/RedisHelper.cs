using Application.Common;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RedisHelper
    {
        private static string _conn = null;
        private static object _lock = new object();
        private static RedisHelper _instance = null;
        private static IConnectionMultiplexer client;
        private static IDatabase _db;
        static RedisHelper()
        {
            string value = ConfigurationManager.AppSettings["redisAddress"];
            _conn = ConfigTools.DESDecrypt(value);
            client = (IConnectionMultiplexer)ConnectionMultiplexer.Connect(_conn, null);
            _db = client.GetDatabase(-1, null);
        }

        #region single
        private RedisHelper() { }
        public static RedisHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            try
                            {
                                _instance = new RedisHelper();
                            }
                            catch { }
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public string GetServerTime()
        {
            DateTime serverTime = client.GetServer("192.168.1.158:6399").Time().ToLocalTime();
            return serverTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 设置字符串键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool StringSet(string key, string value, int dbIndex = 1)
        {
            return client.GetDatabase(dbIndex).StringSet(key, value);
        }

        /// <summary>
        /// 通过键值获取字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string StringGet(string key, int dbIndex = 1)
        {
            try
            {

                return client.GetDatabase(dbIndex).StringGet(key);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置实体键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool SetModel<T>(string key, T t, int dbIndex = 1) where T : class
        {
            try
            {
                string str = JsonConvert.SerializeObject(t);
                return client.GetDatabase(dbIndex).StringSet(key, str);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取实体键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public T GetModel<T>(string key, int dbIndex = 1) where T : class
        {
            try
            {
                string redisValue = client.GetDatabase(dbIndex).StringGet(key);
                return string.IsNullOrEmpty(redisValue) ? default(T) : JsonConvert.DeserializeObject<T>(redisValue);
            }
            catch
            {
                return default(T);
            }

        }

        /// <summary>
        /// 获取实体键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public List<T> GetModelListByPatten<T>(string patten, int dbIndex = 1) where T : class
        {
            List<T> list = new List<T>();
            var keys = client.GetServer(_conn).Keys(dbIndex, patten);
            return GetModelList<T>(keys.ToArray(), dbIndex);
        }

        public List<T> GetModelList<T>(RedisKey[] keys, int dbIndex)
        {
            List<T> strArray = new List<T>();
            try
            {
                RedisValue[] redisValueArray = RedisHelper.client.GetDatabase(-1, (object)null).StringGet(keys, CommandFlags.None);
                for (int index = 0; index < redisValueArray.Length; ++index)
                    strArray.Add(JsonConvert.DeserializeObject<T>((string)redisValueArray[index]));
                return strArray;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 判断是否存在某个键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool IsSet(string key, int dbIndex = 1)
        {
            try
            {
                return client.GetDatabase(dbIndex).KeyExists(key);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="keyStrs"></param>
        /// <returns></returns>
        public string[] StringGetMany(string[] keyStrs)
        {
            int length = keyStrs.Length;
            RedisKey[] keys = new RedisKey[length];
            string[] strArray = new string[length];
            for (int index = 0; index < length; ++index)
                keys[index] = (RedisKey)keyStrs[index];
            try
            {
                RedisValue[] redisValueArray = client.GetDatabase(-1, (object)null).StringGet(keys, CommandFlags.None);
                for (int index = 0; index < redisValueArray.Length; ++index)
                    strArray[index] = (string)redisValueArray[index];
                return strArray;
            }
            catch (Exception)
            {
                return (string[])null;
            }
        }

        /// <summary>
        /// 批量设置
        /// </summary>
        /// <param name="keysStr"></param>
        /// <param name="valuesStr"></param>
        /// <returns></returns>
        public bool StringSetMany(string[] keysStr, string[] valuesStr)
        {
            int length = keysStr.Length;
            KeyValuePair<RedisKey, RedisValue>[] values = new KeyValuePair<RedisKey, RedisValue>[length];
            for (int index = 0; index < length; ++index)
                values[index] = new KeyValuePair<RedisKey, RedisValue>((RedisKey)keysStr[index], (RedisValue)valuesStr[index]);
            return RedisHelper.client.GetDatabase(-1, (object)null).StringSet(values, When.Always, CommandFlags.None);
        }

        public bool ModelSetMany<T>(List<string> keysStr, List<T> values)
        {
            int count = keysStr.Count;
            KeyValuePair<RedisKey, RedisValue>[] values1 = new KeyValuePair<RedisKey, RedisValue>[count];
            for (int index = 0; index < count; ++index)
                values1[index] = new KeyValuePair<RedisKey, RedisValue>((RedisKey)keysStr[index], (RedisValue)JsonConvert.SerializeObject((object)values[index]));
            return RedisHelper.client.GetDatabase(-1, (object)null).StringSet(values1, When.Always, CommandFlags.None);
        }
    }
}
