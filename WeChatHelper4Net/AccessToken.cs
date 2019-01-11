using System;
using System.Collections.Generic;
using System.Text;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API SDK
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 授权访问令牌
    /// </summary>
    public class AccessToken
    {
        private AccessToken() { }

        private static string key(string AppId) { return string.Concat(string.IsNullOrWhiteSpace(AppId) ? Privacy.AppId : AppId, "_AccessToken"); }
        private static string keyState(string AppId) { return string.Concat(key(AppId), "_state"); }

        /// <summary>
        /// 从自定义存储库(接入者的DB、Cache、仓储源)获取AccessToken
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="AppId">AppId</param>
        /// <returns></returns>
        public delegate AccessTokenCacheModel GetAccessTokenFromStorage(DateTime Now, string AppId);
        /// <summary>
        /// 将AccessToken插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="Token">AccessTokenCacheModel</param>
        /// <returns></returns>
        public delegate bool UpdateAccessTokenToStorage(DateTime Now, AccessTokenCacheModel Token);

        /// <summary>
        /// 检查AccessToken是否有效
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="Token">AccessTokenCacheModel</param>
        /// <returns></returns>
        public static bool CheckAccessToken(DateTime Now, AccessTokenCacheModel Token)
        {
            if(null == Token) return false;
            if(string.IsNullOrWhiteSpace(Token.access_token) || Token.expires_in < 1 || string.IsNullOrWhiteSpace(Token.expires_time)) return false;
            return DateTime.Compare(Now.AddMinutes(5), DateTime.SpecifyKind(DateTime.Parse(Token.expires_time), Now.Kind)) < 0;
        }
        private static AccessTokenModel GetAccessTokenFromWeChat(DateTime Now, string AppId, string AppSecret)
        {
            if(string.IsNullOrWhiteSpace(AppId))
                throw new ArgumentNullException(nameof(AppId));
            if(string.IsNullOrWhiteSpace(AppSecret))
                throw new ArgumentNullException(nameof(AppSecret));

            string errorInfo = (string)CacheHelper.GetCache(keyState(AppId));
            if(!string.IsNullOrWhiteSpace(errorInfo) && errorInfo.Contains(Common.error))
                throw new AggregateException(errorInfo + " 如果确认故障已解除，请回收应用程序池后再试");

            if(GlobalFlag.Instance.wxAccessToken_IsBusy)
                throw new ApplicationException("已经有一个请求正在进行，请稍后再试");

            //获取新凭证
            string url = Common.ApiUrl + string.Format("token?grant_type=client_credential&appid={0}&secret={1}", AppId, AppSecret);
            try
            {
                GlobalFlag.Instance.wxAccessToken_IsBusy = true;
                string result = HttpRequestHelper.Get(url);
                AccessTokenModel model = JsonHelper.DeSerialize<AccessTokenModel>(result);
                if(null != model && !string.IsNullOrWhiteSpace(model.access_token) && model.expires_in > 0)
                {
                    GlobalFlag.Instance.wxAccessToken_IsBusy = false;
                    return model;
                }
                else
                {
                    CacheHelper.SetCache(keyState(AppId), string.Concat("Request API token ", Common.error));
                    throw new AggregateException("获取access_token票据失败，url=" + url + "，result=" + result);
                }
            }
            catch(Exception Ex)
            {
                GlobalFlag.Instance.wxAccessToken_IsBusy = false;
                LogHelper.Save(Ex);
                CacheHelper.SetCache(keyState(AppId), string.Concat("Request API token ", Common.error));
                throw Ex;
            }
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="AppId">AppId为空时默认取配置文件appSettings节点key=WeChatAppId</param>
        /// <param name="AppSecret">AppSecret为空时默认取配置文件appSettings节点key=WeChatAppSecret</param>
        /// <param name="GetAccessTokenFromStorage">委托客户端从自定义存储库(接入者的DB、Cache、仓储源)获取AccessToken</param>
        /// <param name="UpdateAccessTokenToStorage">委托客户端将AccessToken插入或更新到自定义存储库(接入者的DB、Cache、仓储源)</param>
        /// <returns></returns>
        public static AccessTokenCacheModel GetAccessToken(DateTime Now, string AppId, string AppSecret, GetAccessTokenFromStorage GetAccessTokenFromStorage = null, UpdateAccessTokenToStorage UpdateAccessTokenToStorage = null)
        {
            AppId = string.IsNullOrWhiteSpace(AppId) ? Privacy.AppId : AppId;
            AppSecret = string.IsNullOrWhiteSpace(AppSecret) ? Privacy.AppSecret : AppSecret;

            var tokenModel = GetAccessTokenFromStorage?.Invoke(Now, AppId);
            if(CheckAccessToken(Now, tokenModel))
            {
                return tokenModel;
            }

            tokenModel = GetAccessTokenFromCache(AppId);
            if(CheckAccessToken(Now, tokenModel))
            {
                return tokenModel;
            }

            var token = GetAccessTokenFromWeChat(Now, AppId, AppSecret);
            tokenModel = new AccessTokenCacheModel(token.expires_in);
            tokenModel.access_token = token.access_token;
            tokenModel.expires_in = token.expires_in;
            tokenModel.appid = AppId;

            UpdateAccessTokenToCache(tokenModel);
            UpdateAccessTokenToStorage?.Invoke(Now, tokenModel);

            return tokenModel;
        }

        #region AccessToken Cache
        private static AccessTokenCacheModel GetAccessTokenFromCache(string AppId)
        {
            object Token = CacheHelper.GetCache(key(AppId));
            return null != Token ? (AccessTokenCacheModel)Token : null;
        }
        private static bool UpdateAccessTokenToCache(AccessTokenCacheModel Token)
        {
            if(null != Token)
            {
                CacheHelper.SetCache(key(Token.appid), Token, Token.expires_in / 60);
                return true;
            }
            return false;
        }
        #endregion
    }

}
