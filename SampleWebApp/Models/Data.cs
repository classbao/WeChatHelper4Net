using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChatHelper4Net.Models;

namespace SampleWebApp.Models
{
    public class Data
    {
        /// <summary>
        /// 从自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public static AccessTokenCacheModel GetAccessTokenByRedis(string AppId)
        {
            // 从自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）

            var AccessToken = new AccessTokenCacheModel();
            AccessToken.appid = "";
            AccessToken.access_token = "";
            AccessToken.errcode = 0;
            AccessToken.errmsg = "";
            AccessToken.expires_in = 0;
            AccessToken.expires_time = "";

            return AccessToken;
        }
        /// <summary>
        /// 从自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public static AccessTokenCacheModel GetAccessTokenByDB(string AppId)
        {
            // 从自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）

            var AccessToken = new AccessTokenCacheModel();
            AccessToken.appid = "";
            AccessToken.access_token = "";
            AccessToken.errcode = 0;
            AccessToken.errmsg = "";
            AccessToken.expires_in = 0;
            AccessToken.expires_time = "";

            return AccessToken;
        }

        /// <summary>
        /// 将票据存到自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool UpdateAccessTokenToRedis(string AppId, AccessTokenCacheModel Token)
        {
            // 存票据到快速数据

            return true;
        }
        /// <summary>
        /// 将票据存到自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）
        /// </summary>
        /// <param name="AppId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool UpdateAccessTokenToDB(string AppId, AccessTokenCacheModel Token)
        {
            // 存票据到数据库

            return true;
        }

    }
}