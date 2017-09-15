using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    public class CacheHelper
    {
        private CacheHelper() { }

        /// <summary>
        /// 获取当前应用程序指定key的Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>y
        public static object GetCache(string key)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache; //Page.Cache访问了HttpContext.Cache，而HttpContext.Cache又直接访问HttpRuntime.Cache
            return objCache[key];
        }

        /// <summary>
        /// 向 System.Web.Caching.Cache 对象插入项，该项带有一个缓存键引用其位置，并使用 System.Web.Caching.CacheItemPriority枚举提供的默认值。
        /// </summary>
        /// <param name="key">用于引用该项的缓存键。</param>
        /// <param name="value">要插入缓存中的对象。</param>
        public static void SetCache(string key, object value)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value);
        }

        /// <summary>
        /// 向 System.Web.Caching.Cache 中插入具有依赖项和到期策略的对象。
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键。</param>
        /// <param name="value">要插入缓存中的对象。</param>
        /// <param name="absoluteExpiration">所插入对象将到期并被从缓存中移除的时间。要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是System.DateTime.Now 作为此参数值。如果使用绝对到期，则 slidingExpiration 参数必须为 System.Web.Caching.Cache.NoSlidingExpiration。</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。如果使用可调到期，则absoluteExpiration 参数必须为 System.Web.Caching.Cache.NoAbsoluteExpiration。</param>
        public static void SetCache(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 向 System.Web.Caching.Cache 中插入具有依赖项和到期策略的对象。
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键。</param>
        /// <param name="value">要插入缓存中的对象。</param>
        /// <param name="minutesExpiration">过期时间(分钟)</param>
        public static void SetCache(string key, object value, int minutesExpiration)
        {
            SetCache(key, value, System.DateTime.UtcNow.AddMinutes(minutesExpiration), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 清除单一键缓存
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveKeyCache(string key)
        {
            try
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                objCache.Remove(key);
            }
            catch { }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            if (_cache.Count > 0)
            {
                List<string> Keys = new List<string>();
                while (CacheEnum.MoveNext())
                {
                    Keys.Add(CacheEnum.Key.ToString());
                }
                foreach (string key in Keys)
                {
                    _cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 以列表形式返回已存在的所有缓存 
        /// </summary>
        /// <returns></returns> 
        public static List<string> CacheKeys()
        {
            List<string> Keys = new List<string>();
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            if (_cache.Count > 0)
            {
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    Keys.Add(CacheEnum.Key.ToString());
                }
            }
            return Keys;
        }

    }
}

