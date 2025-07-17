using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API（微信JS-SDK）
 * 作者：熊学浩
 * 时间：2015-01-12
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 微信JS-SDK
    /// </summary>
    public class JSSDK
    {
        private JSSDK() { }

        private static string key(string AppId) { return string.Concat(string.IsNullOrWhiteSpace(AppId) ? Privacy.AppId : AppId, "_JSApiTicket"); }
        private static string keyState(string AppId) { return string.Concat(key(AppId), "_state"); }

        /// <summary>
        /// 从自定义存储库(接入者的DB、Cache、仓储源)获取Ticket
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="AppId">AppId</param>
        /// <returns></returns>
        public delegate JSApiTicketCacheModel GetJSApiTicketFromStorage(DateTime Now, string AppId);
        /// <summary>
        /// 将Ticket插入或更新到自定义存储库(接入者的DB、Cache、仓储源)
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="Ticket">JSApiTicketCacheModel</param>
        /// <returns></returns>
        public delegate bool UpdateJSApiTicketToStorage(DateTime Now, JSApiTicketCacheModel Ticket);

        /// <summary>
        /// 检查Ticket是否有效
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="Ticket">JSApiTicketCacheModel</param>
        /// <returns></returns>
        public static bool CheckTicket(DateTime Now, JSApiTicketCacheModel Ticket)
        {
            if(null == Ticket) return false;
            if(string.IsNullOrWhiteSpace(Ticket.ticket) || Ticket.expires_in < 1 || string.IsNullOrWhiteSpace(Ticket.expires_time)) return false;
            return DateTime.Compare(Now.AddMinutes(5), DateTime.SpecifyKind(DateTime.Parse(Ticket.expires_time), Now.Kind)) < 0;
        }

        private static JSApiTicketModel GetJSApiTicketFromWeChat(string AppId, string access_token)
        {
            if(string.IsNullOrWhiteSpace(access_token))
                throw new ArgumentNullException(nameof(access_token));

            string errorInfo = (string)CacheHelper.GetCache(keyState(AppId));
            if(!string.IsNullOrWhiteSpace(errorInfo) && errorInfo.Contains(Common.error))
                throw new AggregateException(errorInfo + " 如果确认故障已解除，请回收应用程序池后再试");

            if(GlobalFlag.Instance.wxJSApiTicket_IsBusy)
                throw new ApplicationException("已经有一个请求正在进行，请稍后再试");

            //获取新票据
            string url = Common.ApiUrl + string.Format("ticket/getticket?access_token={0}&type=jsapi", access_token);
            try
            {
                GlobalFlag.Instance.wxJSApiTicket_IsBusy = true;
                string result = HttpRequestHelper.Request(url);
                JSApiTicketModel model = JsonHelper.DeSerialize<JSApiTicketModel>(result);
                if(null != model && !string.IsNullOrWhiteSpace(model.ticket) && model.expires_in > 0)
                {
                    GlobalFlag.Instance.wxJSApiTicket_IsBusy = false;
                    return model;
                }
                else
                {
                    CacheHelper.SetCache(keyState(AppId), string.Concat("Request API Ticket ", Common.error));
                    throw new AggregateException("获取Ticket票据失败，url=" + url + "，result=" + result);
                }
            }
            catch(Exception Ex)
            {
                GlobalFlag.Instance.wxJSApiTicket_IsBusy = false;
                LogHelper.Save(Ex);
                CacheHelper.SetCache(keyState(AppId), string.Concat("Request API Ticket ", Common.error));
                throw Ex;
            }
        }

        #region Ticket Cache
        private static JSApiTicketCacheModel GetTicketFromCache(string AppId)
        {
            object Ticket = CacheHelper.GetCache(key(AppId));
            return null != Ticket ? (JSApiTicketCacheModel)Ticket : null;
        }
        private static bool UpdateTicketToCache(JSApiTicketCacheModel Ticket)
        {
            if(null != Ticket)
            {
                CacheHelper.SetCache(key(Ticket.appid), Ticket, Ticket.expires_in / 60);
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 获取JSApiTicket
        /// </summary>
        /// <param name="Now">当前日期时间</param>
        /// <param name="AppId">AppId为空时默认取配置文件appSettings节点key=WeChatAppId</param>
        /// <param name="access_token">访问令牌</param>
        /// <param name="GetJSApiTicketFromStorage">委托客户端从自定义存储库(接入者的DB、Cache、仓储源)获取JSApiTicket</param>
        /// <param name="UpdateJSApiTicketToStorage">委托客户端将JSApiTicket插入或更新到自定义存储库(接入者的DB、Cache、仓储源)</param>
        /// <returns></returns>
        public static JSApiTicketCacheModel GetJSApiTicket(DateTime Now, string AppId, string access_token, GetJSApiTicketFromStorage GetJSApiTicketFromStorage = null, UpdateJSApiTicketToStorage UpdateJSApiTicketToStorage = null)
        {
            if(string.IsNullOrWhiteSpace(access_token))
                throw new ArgumentNullException(nameof(access_token));
            AppId = string.IsNullOrWhiteSpace(AppId) ? Privacy.AppId : AppId;

            var ticketModel = GetJSApiTicketFromStorage?.Invoke(Now, AppId);
            if(CheckTicket(Now, ticketModel))
            {
                return ticketModel;
            }

            ticketModel = GetTicketFromCache(AppId);
            if(CheckTicket(Now, ticketModel))
            {
                return ticketModel;
            }

            var ticket = GetJSApiTicketFromWeChat(AppId, access_token);
            ticketModel = new JSApiTicketCacheModel(ticket.expires_in);
            ticketModel.ticket = ticket.ticket;
            ticketModel.expires_in = ticket.expires_in;
            ticketModel.appid = AppId;

            UpdateTicketToCache(ticketModel);
            UpdateJSApiTicketToStorage?.Invoke(Now, ticketModel);

            return ticketModel;
        }

        /// <summary>
        /// 获取JS-SDK配置信息
        /// </summary>
        /// <param name="AppId">AppId为空时默认取配置文件appSettings节点key=WeChatAppId</param>
        /// <param name="jsapi_ticket">jsapi_ticket</param>
        /// <param name="url">url（当前网页的URL，不包含#及其后面部分，要用Request.Url.AbsoluteUri，否则会存在编码问题）</param>
        /// <returns></returns>
        public static Models.JSSDK.JSSDKConfig GetConfig(string AppId, string jsapi_ticket, string url)
        {
            if(string.IsNullOrWhiteSpace(jsapi_ticket))
                throw new ArgumentNullException(nameof(jsapi_ticket));
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));
            AppId = string.IsNullOrWhiteSpace(AppId) ? Privacy.AppId : AppId;

            Models.JSSDK.JSSDKConfig config = new Models.JSSDK.JSSDKConfig();
            config.appId = AppId;
            url = Common.CleanUrl(url); //移除url里面#及#后面的部分
            config.timestamp = TimestampHelper.ConvertTime(DateTime.Now);
            config.nonceStr = RandomCode.GenerateRandomCode(16); //生成16位随机字符串，数字，大写英文字母,小写英文字母

            /*
			 * 关于URL编码导致签名错误的问题：
			 * 微信建议：确认url是页面完整的url(请在当前页面alert(location.href.split('#')[0])确认)，包括'http(s)://'部分，以及'？'后面的GET参数部分,但不包括'#'hash后面的部分。
             * 注意必须以http://或https://开头，分别支持80端口和443端口。其它协议或端口都不行！
			 */

            /*
			 * 对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）后，使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串string1。
			 * 这里需要注意的是所有参数名均为小写字符。对string1作sha1加密，字段名和字段值都采用原始值，不进行URL 转义。
			 */
            Dictionary<string, string> WeiXinUrlParameters = new Dictionary<string, string>();
            WeiXinUrlParameters.Add("jsapi_ticket", jsapi_ticket);
            WeiXinUrlParameters.Add("noncestr", config.nonceStr);
            WeiXinUrlParameters.Add("timestamp", config.timestamp.ToString());
            WeiXinUrlParameters.Add("url", url);
            string stringTemp = WeiXinUrlParameters.Sort().ToURLParameter();

            /*
			 * 对string1进行sha1签名，得到signature
			 */
            config.signature = Common.SHA1Encrypt(stringTemp);
            return config;
        }

    }

}