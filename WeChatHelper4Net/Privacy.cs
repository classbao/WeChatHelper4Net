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
        /// </summary>
        public static readonly string PartnerID = "12***01";
        /// <summary>
        /// 财付通商户权限密钥（API密钥）
        /// </summary>
        public static readonly string PartnerKey = "b1***2500"; //"91013efa57330c52680016f602ff912c";

        /// <summary>
        /// 微信公众号支付请求中用于加密的秘钥paySignKey，对应于支付场景中的AppKey
        /// </summary>
        public static readonly string AppKey = "hn***FM";

        #endregion

        #region 微信公众号
        /// <summary>
        /// 微信公众号域名（结尾不包含“/”）
        /// </summary>
        public static readonly string wxDomainName = ConfigHelper.GetAppSetting("WeiXinDomainName");
        /// <summary>
        /// 公众微信号名称
        /// </summary>
        public static readonly string wxName = ConfigHelper.GetAppSetting("WeiXinName");
        /// <summary>
        /// 公众微信号
        /// </summary>
        public static readonly string wxNumber = ConfigHelper.GetAppSetting("WeiXinNumber");
        /// <summary>
        /// 公众微信号（原始ID）
        /// </summary>
        public static readonly string wxId = ConfigHelper.GetAppSetting("WeiXinId");



        /// <summary>
        /// 开发者ID：AppID(应用ID，微信公众号身份的唯一标识)
        /// </summary>
        public static readonly string AppId = ConfigHelper.GetAppSetting("WeixinAppId");
        /// <summary>
        /// 开发者ID：AppSecret(应用密钥)
        /// </summary>
        public static readonly string AppSecret = ConfigHelper.GetAppSetting("WeixinAppSecret");

        #endregion

    }
}