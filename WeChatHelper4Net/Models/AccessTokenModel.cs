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
        /// <summary>
        /// 用户的唯一标识，未关注用户获取不到
        /// </summary>
        [DataMember(IsRequired = false)]
        public string openid { get; set; }
        /// <summary>
        /// 用户的唯一标识，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public int sex { get; set; }
        /// <summary>
        /// ，未关注用户获取不到
        /// </summary>
        [DataMember(IsRequired = false)]
        public string language { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public string province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public string city { get; set; }
        /// <summary>
        /// 国家，如中国为CN，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// ，未关注用户获取不到
        /// </summary>

        [DataMember(IsRequired = false)]
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom），未关注用户获取不到
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> privilege { get; set; }

        /// <summary>
        /// 绑定开放平台后才会有UnionID
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。未关注用户获取不到
        /// </summary>
        [DataMember(IsRequired = false)]
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