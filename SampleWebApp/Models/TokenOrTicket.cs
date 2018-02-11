using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WeChatHelper4Net;
using WeChatHelper4Net.Models;

namespace SampleWebApp.Models
{
    public class TokenOrTicket
    {
        /// <summary>
        /// 用户自定义获取AccessToken
        /// </summary>
        /// <returns></returns>
        public static AccessTokenCacheModel GetAccessToken()
        {
            AccessToken.GetAccessTokenFromStorage get = delegate (DateTime Now, string AppId)
            {
                //根据AppId从自定义存储库(接入者的DB、Cache、仓储源)获取AccessToken
                /*
                 * return GetAccessTokenByCache(AppId);
                 * return GetAccessTokenByRedis(AppId);
                 * return GetAccessTokenByMemcache(AppId);
                 * return GetAccessTokenByMongodb(AppId);
                 * return GetAccessTokenByDB(AppId);
                 * ……
                 */

                return null;
            };
            AccessToken.UpdateAccessTokenToStorage update = delegate (DateTime now, AccessTokenCacheModel Token)
            {
                //根据Token.appid将AccessToken插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
                /*
                 * return UpdateAccessTokenByCache(Token.appid, Token);
                 * return UpdateAccessTokenByRedis(Token.appid, Token);
                 * return UpdateAccessTokenByMemcache(Token.appid, Token);
                 * return UpdateAccessTokenByMongodb(Token.appid, Token);
                 * return UpdateAccessTokenByDB(Token.appid, Token);
                 * ……
                 */

                return true;
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
        public static JSApiTicketCacheModel GetJSApiTicket()
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

                return null;
            };
            JSSDK.UpdateJSApiTicketToStorage update = delegate (DateTime now, JSApiTicketCacheModel Ticket)
            {
                //根据Ticket.appid将JSApiTicket插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
                /*
                 * return UpdateJSApiTicketByCache(Ticket.appid, Ticket);
                 * return UpdateJSApiTicketByRedis(Ticket.appid, Ticket);
                 * return UpdateJSApiTicketByMemcache(Ticket.appid, Ticket);
                 * return UpdateJSApiTicketByMongodb(Ticket.appid, Ticket);
                 * return UpdateJSApiTicketByDB(Ticket.appid, Ticket);
                 * ……
                 */

                return true;
            };


            string appId = ConfigurationManager.AppSettings["WeChatAppId"].ToString(); //AppId为空时默认取配置文件appSettings节点key=WeChatAppId
            string access_token = TokenOrTicket.GetAccessToken().access_token;

            var token = JSSDK.GetJSApiTicket(DateTime.Now, appId, access_token, get, update);
            if(null != token)
            {
                return token;
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

            string jsapi_ticket = Models.TokenOrTicket.GetJSApiTicket().ticket;
            var JsConfig = JSSDK.GetConfig(appId, jsapi_ticket, url);
            return JsConfig;
        }

    }
}