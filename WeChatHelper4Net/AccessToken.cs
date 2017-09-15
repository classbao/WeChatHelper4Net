using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
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

        private static string key = Common.WeiXinId + "WeiXinAccessToken";
        private static string keyState = key + "_state";

        private static bool CheckAccessToken(AccessTokenCacheModel token, DateTime now)
        {
            if(null == token) return false;
            if(string.IsNullOrWhiteSpace(token.access_token) || token.expires_in < 1 || string.IsNullOrWhiteSpace(token.expires_time)) return false;
            if(token.appid != Privacy.AppId) return false;
            if(DateTime.Compare(now.AddMinutes(10), DateTime.SpecifyKind(DateTime.Parse(token.expires_time), now.Kind)) < 0)
            {
                return true;
            }
            return false;
        }

        private static bool UpdateAccessTokenByMemCache(AccessTokenCacheModel token, string key)
        {
            bool flag = false;
            try
            {
                //flag = PageUtility.SetMemCacheBool(key, token, 40);
                if(!flag)
                {
                    /*
                    if (!PageUtility.DeleteMemcacheByKey(key))
                    {
                        LogHelper.Save("WeChatHelper4Net > AccessToken > GetAccessToken()  更新微信数字签名异常！", "AccessToken", LogType.Error, LogTime.minute);
                        CacheHelper.SetCache(keyState, "MemCache Server error");
                        WeChatHelper4Net.SendMessage.TextToUser("o--NRtx58MS4JX9ilO_BV-VjBAGU", "更新微信数字签名异常AccessToken", DateTime.Now);
                        WeChatHelper4Net.SendMessage.TextToUser("o--NRt9AV2wXeTVl-BLdUzdxtGhQ", "更新微信数字签名异常AccessToken", DateTime.Now);
                        WeChatHelper4Net.SendMessage.TextToUser("o--NRt5I6jmzBBhIkrOWSqsqXVb4", "更新微信数字签名异常AccessToken", DateTime.Now);
                    }
                    */
                }
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex, "WeChatHelper4Net > AccessToken > UpdateAccessTokenByMemCache()  更新微信数字签名异常！", "AccessToken", LogTime.minute);
            }
            return flag;
        }
        private static AccessTokenCacheModel GetAccessTokenFromDB()
        {
            try
            {
                var wxtoken = chinahxmedia.BLL.BLLRepository.wxtokenBLL.LoadByName(nameof(AccessToken), Privacy.AppId);
                if(null != wxtoken && !string.IsNullOrWhiteSpace(wxtoken.token))
                {
                    AccessTokenCacheModel tokenModel = new AccessTokenCacheModel();
                    tokenModel.appid = wxtoken.appid;
                    tokenModel.access_token = wxtoken.token;
                    tokenModel.expires_in = wxtoken.expires_in;
                    tokenModel.expires_time = (null != wxtoken.expires_time && wxtoken.expires_time.Year > 1970) ? wxtoken.expires_time.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                    return tokenModel;
                }
            }
            catch(Exception Ex)
            {
                LogHelper.Save("GetAccessTokenFromDB>" + Ex.Message, nameof(AccessToken), LogType.Error, LogTime.hour);
                CacheHelper.SetCache(keyState, string.Concat("DB Server ", Common.error));
                //throw Ex;
            }
            return null;
        }
        private static bool UpdateAccessTokenToDB(AccessTokenCacheModel token)
        {
            if(null == token || string.IsNullOrWhiteSpace(token.access_token)) return false;
            try
            {
                chinahxmedia.Model.wxtoken wxtoken = new chinahxmedia.Model.wxtoken()
                {
                    name = nameof(AccessToken),
                    appid = token.appid,

                    token = token.access_token,
                    expires_in = token.expires_in,
                    expires_time = DateTime.SpecifyKind(DateTime.Parse(token.expires_time), DateTime.Now.Kind),
                    creationtime = DateTime.Now
                };

                return chinahxmedia.BLL.BLLRepository.wxtokenBLL.UpdateByName(nameof(AccessToken), Privacy.AppId, wxtoken, TimeSpan.FromSeconds(token.expires_in));
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex, "AccessToken，更新微信数字签名异常！", nameof(AccessToken), LogTime.hour);
                CacheHelper.SetCache(keyState, string.Concat("DB Server ", Common.error));
                return false;
            }
        }

        /// <summary>
        /// 获取有效Access_Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken(DateTime now)
        {
            AccessTokenCacheModel tokenModel = null;
            object token = null;
            //LogHelper.Save("获取access_token票据，IISCache开始", nameof(AccessToken), LogType.Report, LogTime.hour);
            token = CacheHelper.GetCache(key);
            if(null != token)
                tokenModel = (AccessTokenCacheModel)token;
            if(CheckAccessToken(tokenModel, now))
                return tokenModel.access_token;
            //LogHelper.Save("获取access_token票据，IISCache结束，没有有效的票据。wxAccessToken_IsBusy=" + GlobalFlag.Instance.wxAccessToken_IsBusy, nameof(AccessToken), LogType.Report, LogTime.hour);

            //tokenModel = (AccessTokenXMLModel)PageUtility.GetMemCache(key);
            //if (CheckAccessToken(tokenModel, now))
            //{
            //	return tokenModel.access_token;
            //}

            //LogHelper.Save("获取access_token票据，GetAccessTokenFromDB开始", nameof(AccessToken), LogType.Report, LogTime.hour);
            tokenModel = GetAccessTokenFromDB();
            //LogHelper.Save("获取access_token票据，GetAccessTokenFromDB=" + JsonHelper.Serialize(tokenModel), nameof(AccessToken), LogType.Report, LogTime.hour);
            if(CheckAccessToken(tokenModel, now))
            {
                CacheHelper.SetCache(key, tokenModel, tokenModel.expires_in / 60);
                //UpdateAccessTokenByMemCache(tokenModel, key);
                return tokenModel.access_token;
            }
            //LogHelper.Save("获取access_token票据，GetAccessTokenFromDB结束，没有有效的票据。wxAccessToken_IsBusy=" + GlobalFlag.Instance.wxAccessToken_IsBusy, nameof(AccessToken), LogType.Report, LogTime.hour);

            string errorInfo = (string)CacheHelper.GetCache(keyState);
            if(!string.IsNullOrWhiteSpace(errorInfo) && errorInfo.Contains(Common.error))
                throw new AggregateException(errorInfo);

            //LogHelper.Save("获取access_token票据，wxAccessToken_IsBusy=" + GlobalFlag.Instance.wxAccessToken_IsBusy.ToString(), nameof(AccessToken), LogType.Report, LogTime.hour);

            if(GlobalFlag.Instance.wxAccessToken_IsBusy) return GetAccessToken(now);

            //获取新凭证
            string url = Common.ApiUrl + string.Format("token?grant_type=client_credential&appid={0}&secret={1}", Privacy.AppId, Privacy.AppSecret);
            //LogHelper.Save("获取access_token票据开始，url=" + url, nameof(AccessToken), LogType.Report, LogTime.hour);
            try
            {
                GlobalFlag.Instance.wxAccessToken_IsBusy = true;
                string result = HttpRequestHelper.Get(url);  //HttpRequestHelper.Request(url);
                //LogHelper.Save("获取access_token票据结果，wxAccessToken_IsBusy=" + GlobalFlag.Instance.wxAccessToken_IsBusy + "，result=" + result, nameof(AccessToken), LogType.Report, LogTime.hour);
                AccessTokenModel model = JsonHelper.DeSerialize<AccessTokenModel>(result);
                if(null != model && !string.IsNullOrWhiteSpace(model.access_token) && model.expires_in > 0)
                {
                    tokenModel = new AccessTokenCacheModel(model.expires_in);
                    tokenModel.access_token = model.access_token;
                    tokenModel.expires_in = model.expires_in;
                    tokenModel.appid = Privacy.AppId;

                    try
                    {
                        CacheHelper.SetCache(key, tokenModel, tokenModel.expires_in / 60);
                        //UpdateAccessTokenByMemCache(tokenModel, key);
                        UpdateAccessTokenToDB(tokenModel);
                    }
                    catch(Exception Ex)
                    {
                        LogHelper.Save("AccessToken，存储数据报错！ 【Message】=" + Ex.Message, nameof(AccessToken), LogType.Error, LogTime.hour);
                    }
                    //LogHelper.Save("获取access_token票据完成，wxAccessToken_IsBusy=" + GlobalFlag.Instance.wxAccessToken_IsBusy + "，access_token=" + tokenModel.access_token, nameof(AccessToken), LogType.Report, LogTime.hour);
                    GlobalFlag.Instance.wxAccessToken_IsBusy = false;
                    return tokenModel.access_token;
                }
                else
                {
                    LogHelper.Save("获取access_token票据失败，url=" + url + "，result=" + result, nameof(AccessToken), LogType.Error, LogTime.hour);
                    CacheHelper.SetCache(keyState, string.Concat("Request API ", Common.error));
                }
            }
            catch(Exception Ex)
            {
                GlobalFlag.Instance.wxAccessToken_IsBusy = false;
                LogHelper.Save(Ex);
                CacheHelper.SetCache(keyState, string.Concat("Request API ", Common.error));
                throw Ex;
            }

            return string.Empty;
        }


    }
}
