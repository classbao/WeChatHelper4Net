using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
    [Serializable]
    [DataContract]
    public class AccessTokenModel : RequestResult_errmsg
    {
        /// <summary>
        /// access_token的存储至少要保留512个字符空间
        /// </summary>
        [DataMember(IsRequired = false)]
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        [DataMember(IsRequired = false)]
        public int expires_in { get; set; }
    }

    /// <summary>
    /// access_token缓存模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class AccessTokenCacheModel : AccessTokenModel
    {
        [DataMember(IsRequired = false)]
        public string appid { get; set; }

        /// <summary>
        /// 凭证的到期时间（值自定义）
        /// </summary>
        [DataMember(IsRequired = false)]
        public string expires_time { get; set; }

        public AccessTokenCacheModel() { }
        public AccessTokenCacheModel(int expires_in)
        {
            if (expires_in > 0)
            {
                base.expires_in = expires_in;
                expires_time = DateTime.Now.AddSeconds(expires_in).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }


    [Serializable]
    [DataContract]
    public class PageOAuth_AccessToken : AccessTokenModel
    {
        [DataMember(IsRequired = false)]
        public string refresh_token { get; set; }

        [DataMember(IsRequired = false)]
        public string openid { get; set; }
        [DataMember(IsRequired = false)]
        public string unionid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        [DataMember(IsRequired = false)]
        public string scope { get; set; }
    }

    [Serializable]
    [DataContract]
    public class UserInfo : RequestResult_errmsg
    {
        [DataMember(IsRequired = true)]
        public string openid { get; set; }

        [DataMember(IsRequired = true)]
        public string nickname { get; set; }

        [DataMember(IsRequired = true)]
        public int sex { get; set; }

        [DataMember(IsRequired = true)]
        public string province { get; set; }

        [DataMember(IsRequired = true)]
        public string city { get; set; }

        [DataMember(IsRequired = true)]
        public string country { get; set; }

        [DataMember(IsRequired = true)]
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        [DataMember(IsRequired = true)]
        public List<string> privilege { get; set; }

        [DataMember(IsRequired = true)]
        public string unionid { get; set; }
    }




    /// <summary>
    /// jsapi_ticket模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class JSApiTicketModel : RequestResult_errmsg
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [DataMember(IsRequired = false)]
        public string ticket { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        [DataMember(IsRequired = false)]
        public int expires_in { get; set; }
    }
    
    /// <summary>
    /// jsapi_ticket缓存模型
    /// </summary>
    [Serializable]
    public class JSApiTicketCacheModel: JSApiTicketModel
    {
        [DataMember(IsRequired = false)]
        public string appid { get; set; }

        /// <summary>
        /// 凭证的到期时间（值自定义）
        /// </summary>
        [DataMember(IsRequired = false)]
        public string expires_time { get; set; }

        public JSApiTicketCacheModel() { }
        public JSApiTicketCacheModel(int expires_in)
        {
            if (expires_in > 0)
            {
                base.expires_in = expires_in;
                expires_time = DateTime.Now.AddSeconds(expires_in).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }


}