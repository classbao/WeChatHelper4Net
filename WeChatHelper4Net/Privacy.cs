using System;
using WeChatHelper4Net.Extend;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 公众账号（服务号）开发者凭据，隐私设置
    /// </summary>
    internal sealed class Privacy
    {
        private Privacy() { }

        #region 微信商户&财付通账号
        /// <summary>
        /// 商户号（财付通商户身份标识 MCHID）
        /// https://pay.weixin.qq.com/wiki/doc/api/H5.php?chapter=9_20&index=1
        /// </summary>
        public static readonly string PartnerID = ConfigHelper.GetAppSetting("WeChatPartnerID"); //"12***01";
        /// <summary>
        /// 财付通商户权限密钥（API密钥）
        /// https://pay.weixin.qq.com/wiki/doc/api/H5.php?chapter=4_3
        /// </summary>
        public static readonly string PartnerKey = ConfigHelper.GetAppSetting("WeChatPartnerKey"); //"91013efa57330c52680016f602ff912c";

        /// <summary>
        /// 微信公众号支付请求中用于加密的秘钥paySignKey，对应于支付场景中的AppKey
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi_sl.php?chapter=7_7&index=6
        /// </summary>
        public static readonly string AppKey = ConfigHelper.GetAppSetting("WeChatpaySignKey"); //"hn***FM";

        #endregion

        #region 微信公众号
        /// <summary>
        /// 微信公众号域名（结尾不包含“/”）
        /// </summary>
        public static readonly string WeChatDomainName = ConfigHelper.GetAppSetting("WeChatDomainName");
        /// <summary>
        /// 公众微信号名称
        /// </summary>
        public static readonly string WeChatName = ConfigHelper.GetAppSetting("WeChatName");
        /// <summary>
        /// 公众微信号
        /// </summary>
        public static readonly string WeChatNumber = ConfigHelper.GetAppSetting("WeChatNumber");
        /// <summary>
        /// 公众微信号（原始ID）
        /// </summary>
        public static readonly string WeChatId = ConfigHelper.GetAppSetting("WeChatId");
        /// <summary>
        /// 开发者ID：AppID(应用ID，微信公众号身份的唯一标识)
        /// </summary>
        public static readonly string AppId = ConfigHelper.GetAppSetting("WeChatAppId");
        /// <summary>
        /// 开发者ID：AppSecret(应用密钥)
        /// </summary>
        public static readonly string AppSecret = ConfigHelper.GetAppSetting("WeChatAppSecret");

        #endregion

    }
}