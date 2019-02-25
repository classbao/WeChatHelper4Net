using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WeChatHelper4Net;
using WeChatHelper4Net.Models;

namespace SampleWebApp.Models
{
    /// <summary>
    /// 用于微信WeChatHelper4Net调用用户自定义的票据存储
    /// 开源社区：https://github.com/classbao/WeChatHelper4Net
    /// </summary>
    public class WeChatTokenOrTicket
    {
        /// <summary>
        /// 用户自定义获取AccessToken
        /// </summary>
        /// <returns></returns>
        public static AccessTokenCacheModel GetAccessToken()
        {
            AccessToken.GetAccessTokenFromStorage get = delegate (DateTime Now, string AppId)
            {
                /* 根据AppId从自定义存储库(接入者的DB、Cache、仓储源)获取AccessToken
                 * return GetAccessTokenByCache(AppId);
                 * return GetAccessTokenByRedis(AppId);
                 * return GetAccessTokenByMemcache(AppId);
                 * return GetAccessTokenByMongodb(AppId);
                 * return GetAccessTokenByDB(AppId);
                 * ……
                 */

                /***** 示例·开始 *****/
                AccessTokenCacheModel AccessToken = null;
                /* 先从自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）*/
                object TokenFromCache = CacheHelper.GetCache("WeChatAccessToken_" + AppId);
                AccessToken = null != TokenFromCache ? (AccessTokenCacheModel)TokenFromCache : null;
                if(WeChatHelper4Net.AccessToken.CheckAccessToken(Now, AccessToken))
                {
                    return AccessToken;
                }
                /* 如果快速数据没有，再从自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）*/
                //var TokenFromDB = BLLRepository.wechatManageBLL.GetTokenOrTicket(AppId, "AccessToken");
                //if(null != TokenFromDB)
                //{
                //    AccessToken = new AccessTokenCacheModel()
                //    {
                //        appid = TokenFromDB.appid,
                //        access_token = TokenFromDB.access_token,
                //        expires_in = TokenFromDB.expires_in,
                //        expires_time = TokenFromDB.expires_time,
                //        errcode = TokenFromDB.errcode,
                //        errmsg = TokenFromDB.errmsg
                //    };
                //    if(WeChatHelper4Net.AccessToken.CheckAccessToken(Now, AccessToken))
                //    {
                //        CacheHelper.SetCache("WeChatAccessToken_" + AccessToken.appid, AccessToken, AccessToken.expires_in / 60);
                //        return AccessToken;
                //    }
                //}
                /***** 示例·结束 *****/

                return null;
            };
            AccessToken.UpdateAccessTokenToStorage update = delegate (DateTime now, AccessTokenCacheModel Token)
            {
                bool result = false;

                /* 根据Token.appid将AccessToken插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
                 * result = UpdateAccessTokenToCache(Token.appid, Token);
                 * result = UpdateAccessTokenToRedis(Token.appid, Token);
                 * result = UpdateAccessTokenToMemcache(Token.appid, Token);
                 * result = UpdateAccessTokenToMongodb(Token.appid, Token);
                 * result = UpdateAccessTokenToDB(Token.appid, Token);
                 * ……
                 */

                /***** 示例·开始 *****/
                if(null != Token && !string.IsNullOrWhiteSpace(Token.appid) && Token.expires_in > 60)
                {
                    /* 先将票据存到自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）*/
                    CacheHelper.SetCache("WeChatAccessToken_" + Token.appid, Token, Token.expires_in / 60);

                    //var entity = new TbWeChatTokenOrTicketModel()
                    //{
                    //    appid = Token.appid,
                    //    access_token = Token.access_token,
                    //    expires_in = Token.expires_in,
                    //    expires_time = Token.expires_time,
                    //    errcode = Token.errcode,
                    //    errmsg = Token.errmsg,
                    //    type = "AccessToken",
                    //    UpdateTime = DateTime.Now
                    //};
                    ///* 为了避免快速数据丢失，再将票据存到自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）*/
                    //result = BLLRepository.wechatManageBLL.UpdateTokenOrTicket(entity);
                }
                /***** 示例·结束 *****/

                return result;
            };

            string appId = ConfigurationManager.AppSettings["WeChatAppId"].ToString(); //AppId为空时默认取配置文件appSettings节点key=WeChatAppId
            string appSecret = ConfigurationManager.AppSettings["WeChatAppSecret"].ToString(); //AppSecret为空时默认取配置文件appSettings节点key=WeChatAppSecret

            var token = AccessToken.GetAccessToken(DateTime.Now, appId, appSecret, get, update);
            if(null != token)
            {
                return token;
            }
            else
            {
                return default(AccessTokenCacheModel);
            }

        }

        /// <summary>
        /// 用户自定义获取JSApiTicket
        /// </summary>
        /// <returns></returns>
        private static JSApiTicketCacheModel GetJSApiTicket()
        {
            JSSDK.GetJSApiTicketFromStorage get = delegate (DateTime Now, string AppId)
            {
                //根据AppId从自定义存储库(接入者的DB、Cache、仓储源)获取JSApiTicket
                /*
                    * return GetJSApiTicketByCache(AppId);
                    * return GetJSApiTicketByRedis(AppId);
                    * return GetJSApiTicketByMemcache(AppId);
                    * return GetJSApiTicketByMongodb(AppId);
                    * return GetJSApiTicketByDB(AppId);
                    * ……
                    */

                /***** 示例·开始 *****/
                JSApiTicketCacheModel Ticket = null;
                /* 先从自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）*/
                object TokenFromCache = CacheHelper.GetCache("WeChatJSApiTicket_" + AppId);
                Ticket = null != TokenFromCache ? (JSApiTicketCacheModel)TokenFromCache : null;
                if(WeChatHelper4Net.JSSDK.CheckTicket(Now, Ticket))
                {
                    return Ticket;
                }
                /* 如果快速数据没有，再从自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）*/
                //var TokenFromDB = BLLRepository.wechatManageBLL.GetTokenOrTicket(AppId, "JSApiTicket");
                //if(null != TokenFromDB)
                //{
                //    Ticket = new JSApiTicketCacheModel()
                //    {
                //        appid = TokenFromDB.appid,
                //        ticket = TokenFromDB.access_token,
                //        expires_in = TokenFromDB.expires_in,
                //        expires_time = TokenFromDB.expires_time,
                //        errcode = TokenFromDB.errcode,
                //        errmsg = TokenFromDB.errmsg
                //    };
                //    if(WeChatHelper4Net.JSSDK.CheckTicket(Now, Ticket))
                //    {
                //        CacheHelper.SetCache("WeChatJSApiTicket_" + Ticket.appid, Ticket, Ticket.expires_in / 60);
                //        return Ticket;
                //    }
                //}
                /***** 示例·结束 *****/

                return null;
            };
            JSSDK.UpdateJSApiTicketToStorage update = delegate (DateTime now, JSApiTicketCacheModel Ticket)
            {
                bool result = false;

                //根据Ticket.appid将JSApiTicket插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
                /*
                    * result = UpdateJSApiTicketToCache(Ticket.appid, Ticket);
                    * result = UpdateJSApiTicketToRedis(Ticket.appid, Ticket);
                    * result = UpdateJSApiTicketToMemcache(Ticket.appid, Ticket);
                    * result = UpdateJSApiTicketToMongodb(Ticket.appid, Ticket);
                    * result = UpdateJSApiTicketToDB(Ticket.appid, Ticket);
                    * ……
                    */

                /***** 示例·开始 *****/
                if(null != Ticket && !string.IsNullOrWhiteSpace(Ticket.appid) && Ticket.expires_in > 60)
                {
                    /* 先将票据存到自己系统的快速获取已存在的票据（Cache、Redis，Memcache等）*/
                    CacheHelper.SetCache("WeChatJSApiTicket_" + Ticket.appid, Ticket, Ticket.expires_in / 60);

                    //var entity = new TbWeChatTokenOrTicketModel()
                    //{
                    //    appid = Ticket.appid,
                    //    access_token = Ticket.ticket,
                    //    expires_in = Ticket.expires_in,
                    //    expires_time = Ticket.expires_time,
                    //    errcode = Ticket.errcode,
                    //    errmsg = Ticket.errmsg,
                    //    type = "JSApiTicket",
                    //    UpdateTime = DateTime.Now
                    //};
                    /* 为了避免快速数据丢失，再将票据存到自己系统的稳定数据库获取已存在的票据（MySQL，SQLServer，Oracle等）*/
                    //result = BLLRepository.wechatManageBLL.UpdateTokenOrTicket(entity);
                }
                /***** 示例·结束 *****/

                return result;
            };


            string appId = ConfigurationManager.AppSettings["WeChatAppId"].ToString(); //AppId为空时默认取配置文件appSettings节点key=WeChatAppId
            string access_token = WeChatTokenOrTicket.GetAccessToken().access_token;

            var ticket = JSSDK.GetJSApiTicket(DateTime.Now, appId, access_token, get, update);
            if(null != ticket)
            {
                return ticket;
            }
            else
            {
                return default(JSApiTicketCacheModel);
            }

        }

        /// <summary>
        /// 获取JS-SDK配置信息
        /// </summary>
        /// <param name="url">url（当前网页的URL，不包含#及其后面部分，要用Request.Url.AbsoluteUri，否则会存在编码问题）</param>
        /// <returns></returns>
        public static WeChatHelper4Net.Models.JSSDK.JSSDKConfig GetJSConfig(string url)
        {
            string appId = ConfigurationManager.AppSettings["WeChatAppId"].ToString(); //AppId为空时默认取配置文件appSettings节点key=WeChatAppId

            string jsapi_ticket = Models.WeChatTokenOrTicket.GetJSApiTicket().ticket;
            var JsConfig = JSSDK.GetConfig(appId, jsapi_ticket, url);
            return JsConfig;
        }

    }
}