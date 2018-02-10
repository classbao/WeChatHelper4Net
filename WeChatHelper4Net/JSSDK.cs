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

        private static string key = Common.WeiXinId + "WeiXinJSApiTicket";
        private static string keyState = key + "_state";

        private static bool CheckTicket(JSApiTicketCacheModel ticket, DateTime now)
        {
            if (null == ticket) return false;
            if (string.IsNullOrWhiteSpace(ticket.ticket) || ticket.expires_in < 1 || string.IsNullOrWhiteSpace(ticket.expires_time)) return false;
            if (ticket.appid != Privacy.AppId) return false;
            if (DateTime.Compare(now.AddMinutes(10), DateTime.SpecifyKind(DateTime.Parse(ticket.expires_time), now.Kind)) < 0)
            {
                return true;
            }
            return false;
        }
        
        private static JSApiTicketCacheModel GetTicketFromDB()
        {
            try
            {
                var ticket = com.cidtechBiz.Activity.WeChatService.Instance.GetAccessToken(2);
                if (null != ticket && !string.IsNullOrWhiteSpace(ticket.Token))
                {
                    JSApiTicketCacheModel ticketModel = new JSApiTicketCacheModel();
                    ticketModel.appid = Privacy.AppId;
                    ticketModel.ticket = ticket.Token;
                    ticketModel.expires_in = 7200;
                    var expires_time = Common.ConvertTime(Convert.ToInt32(ticket.Timestamp));
                    ticketModel.expires_time = (null != expires_time && expires_time.Year > 1970) ? expires_time.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                    return ticketModel;
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Save("JSSDK，读取数据反序列化为对象报错！ ", "JSApiTicket", LogType.Error, LogTime.hour);
                CacheHelper.SetCache(keyState, string.Concat("DB Server ", Common.error));
                throw Ex;
            }
            return null;
        }
        private static bool UpdateTicketToDB(JSApiTicketCacheModel ticket)
        {
            if (null == ticket || string.IsNullOrWhiteSpace(ticket.ticket)) return false;
            try
            {
                com.cidtechBiz.Activity.Access_token wxtoken = new com.cidtechBiz.Activity.Access_token()
                {
                    //name = "JSApiTicket",
                    //appid = ticket.appid,

                    //token = ticket.ticket,
                    //expires_in = ticket.expires_in,
                    //expires_time = DateTime.SpecifyKind(DateTime.Parse(ticket.expires_time), DateTime.Now.Kind),
                    //creationtime = DateTime.Now

                    Id = 2,
                    Token = ticket.ticket,
                    Timestamp = Common.ConvertTime(DateTime.Now).ToString(),
                    ModifyDate = DateTime.Now,
                    TokenType = 2
                };

                return com.cidtechBiz.Activity.WeChatService.Instance.ModifyToken(wxtoken);
            }
            catch (Exception Ex)
            {
                LogHelper.Save(Ex, "JSSDK，更新微信数字签名异常！", "JSApiTicket", LogTime.minute);
                CacheHelper.SetCache(keyState, string.Concat("DB Server ", Common.error));
                return false;
            }
        }
        
        private static string GetJSApiTicket(DateTime now)
        {
            JSApiTicketCacheModel ticketModel = null;
            object ticket = null;
            //LogHelper.Save("获取jsapi_ticket票据，IISCache开始", nameof(JSSDK), LogType.Report, LogTime.hour);
            ticket = CacheHelper.GetCache(key);
            if (null != ticket)
                ticketModel = (JSApiTicketCacheModel)ticket;
            if (CheckTicket(ticketModel, now))
                return ticketModel.ticket;
            //LogHelper.Save("获取jsapi_ticket票据，IISCache结束，没有有效的票据。wxJSApiTicket_IsBusy=" + GlobalFlag.Instance.wxJSApiTicket_IsBusy, nameof(JSSDK), LogType.Report, LogTime.hour);

            //LogHelper.Save("获取jsapi_ticket票据，GetTicketFromDB开始", nameof(JSSDK), LogType.Report, LogTime.hour);
            ticketModel = GetTicketFromDB();
            //LogHelper.Save("获取jsapi_ticket票据，GetTicketFromDB=" + JsonHelper.Serialize(ticketModel), nameof(JSSDK), LogType.Report, LogTime.hour);
            if (CheckTicket(ticketModel, now))
            {
                CacheHelper.SetCache(key, ticketModel, ticketModel.expires_in / 60);
                return ticketModel.ticket;
            }
            //LogHelper.Save("获取jsapi_ticket票据，GetTicketFromDB结束，没有有效的票据。wxJSApiTicket_IsBusy=" + GlobalFlag.Instance.wxJSApiTicket_IsBusy, nameof(JSSDK), LogType.Report, LogTime.hour);

            string errorInfo = (string)CacheHelper.GetCache(keyState);
            if (!string.IsNullOrWhiteSpace(errorInfo) && errorInfo.Contains(Common.error))
                throw new AggregateException(errorInfo);

            //LogHelper.Save("获取jsapi_ticket票据，wxJSApiTicket_IsBusy=" + GlobalFlag.Instance.wxJSApiTicket_IsBusy.ToString(), nameof(JSSDK), LogType.Report, LogTime.hour);

            if (GlobalFlag.Instance.wxJSApiTicket_IsBusy) return GetJSApiTicket(now);

            string access_token = AccessToken.GetAccessToken(now);
            if (string.IsNullOrWhiteSpace(access_token)) return string.Empty;

            //获取新凭证
            string url = Common.ApiUrl + string.Format("ticket/getticket?access_token={0}&type=jsapi", access_token);
            //LogHelper.Save("获取jsapi_ticket票据开始，url=" + url, nameof(JSSDK), LogType.Report, LogTime.hour);
            try
            {
                GlobalFlag.Instance.wxJSApiTicket_IsBusy = true;
                string result = HttpRequestHelper.Request(url);
                //LogHelper.Save("获取jsapi_ticket票据结果，wxJSApiTicket_IsBusy=" + GlobalFlag.Instance.wxJSApiTicket_IsBusy + "，result=" + result, nameof(JSSDK), LogType.Report, LogTime.hour);
                JSApiTicketModel model = JsonHelper.DeSerialize<JSApiTicketModel>(result);
                if (null != model && !string.IsNullOrWhiteSpace(model.ticket) && model.expires_in > 0)
                {
                    ticketModel = new JSApiTicketCacheModel(model.expires_in);
                    ticketModel.ticket = model.ticket;
                    ticketModel.expires_in = model.expires_in;
                    ticketModel.appid = Privacy.AppId;

                    try
                    {
                        CacheHelper.SetCache(key, ticketModel, ticketModel.expires_in / 60);
                        UpdateTicketToDB(ticketModel);
                    }
                    catch (Exception Ex)
                    {
                        LogHelper.Save("JSSDK，存储数据报错！ 【Message】=" + Ex.Message, nameof(JSSDK), LogType.Error, LogTime.hour);
                    }
                    //LogHelper.Save("获取jsapi_ticket票据完成，wxJSApiTicket_IsBusy=" + GlobalFlag.Instance.wxJSApiTicket_IsBusy + "，ticket=" + ticketModel.ticket, nameof(JSSDK), LogType.Report, LogTime.hour);
                    GlobalFlag.Instance.wxJSApiTicket_IsBusy = false;
                    return ticketModel.ticket;
                }
                else
                {
                    LogHelper.Save("获取jsapi_ticket票据失败，url=" + url + "，result=" + result, nameof(JSSDK), LogType.Error, LogTime.hour);
                    CacheHelper.SetCache(keyState, string.Concat("Request API ", Common.error));
                }
            }
            catch (Exception Ex)
            {
                GlobalFlag.Instance.wxJSApiTicket_IsBusy = false;
                LogHelper.Save(Ex);
                CacheHelper.SetCache(keyState, string.Concat("Request API ", Common.error));
                throw Ex;
            }

            return string.Empty;
        }

		/// <summary>
		/// 获取JS-SDK配置信息
		/// </summary>
		/// <param name="url">url（当前网页的URL，不包含#及其后面部分，要用Request.Url.AbsoluteUri，否则会存在编码问题）</param>
		/// <param name="now"></param>
		/// <returns></returns>
		public static Models.JSSDK.JSSDKConfig GetConfig(string url, DateTime now)
		{
			/*
			 * 生成签名之前必须先了解一下jsapi_ticket，
			 * jsapi_ticket是公众号用于调用微信JS接口的临时票据。
			 * 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
			 * 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
			 */
			string jsapi_ticket = GetJSApiTicket(now);
			if (string.IsNullOrWhiteSpace(jsapi_ticket))
			{
				return new Models.JSSDK.JSSDKConfig();
				throw new Exception("jsapi_ticket IsNullOrWhiteSpace");
			}

            Models.JSSDK.JSSDKConfig config = new Models.JSSDK.JSSDKConfig();
			url = Common.CleanUrl(url); //移除url里面#及#后面的部分
			config.timestamp = Common.ConvertTime(now);
			config.nonceStr = RandomCode.createRandomCode(16, true); //生成16位随机字符串，数字，大写英文字母,小写英文字母

			//LogHelper.Save("url=" + url, "JSSDKConfig", LogType.Report, LogTime.day); 
			/*
			 * 关于URL编码导致签名错误的问题：
			 * 微信建议：确认url是页面完整的url(请在当前页面alert(location.href.split('#')[0])确认)，包括'http(s)://'部分，以及'？'后面的GET参数部分,但不包括'#'hash后面的部分。
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
			//LogHelper.Save("获取JSSDKConfig：url=" + url + "    jsapi_ticket=" + jsapi_ticket + "    timestamp=" + config.timestamp + "    nonceStr=" + config.nonceStr + "    stringTemp=" + stringTemp + "    signature=" + config.signature, "GetConfig", LogType.Report, LogTime.day);
			return config;
		}

	}
}

//85180262@qq.com（微信：杨茂巍）