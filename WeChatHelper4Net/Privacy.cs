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
        /// 商户号（财付通商户身份标识 MCHID/mch_id）商户申请微信支付后，由微信支付分配的商户收款账号。
        /// https://pay.weixin.qq.com/wiki/doc/api/H5.php?chapter=9_20&index=1
        /// </summary>
        public static readonly string WeChatPay_PartnerID = ConfigHelper.GetAppSetting("WeChatPay_PartnerID"); //"12***01";
        /// <summary>
        /// 财付通商户权限密钥（API密钥/商户APIv2密钥，32位的）交易过程生成签名的密钥，仅保留在商户系统和微信支付后台，不会在网络中传播。商户妥善保管该Key，切勿在网络中传输，不能在其他客户端中存储，保证key不会被泄露。商户可根据邮件提示登录微信商户平台进行设置。也可按以下路径设置：微信商户平台-->账户中心-->账户设置-->API安全-->设置API密钥
        /// https://pay.weixin.qq.com/wiki/doc/api/H5.php?chapter=4_3
        /// </summary>
        public static readonly string WeChatPay_PartnerKey = ConfigHelper.GetAppSetting("WeChatPay_PartnerKey"); //"91013efa57330c52680016f602ff912c";

        /// <summary>
        /// 微信公众号支付请求中用于加密的秘钥paySignKey，对应于支付场景中的AppKey
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi_sl.php?chapter=7_7&index=6
        /// </summary>
        public static readonly string WeChatPay_SignKey = ConfigHelper.GetAppSetting("WeChatPay_SignKey"); //"hn***FM";

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

        /// <summary>
        /// 自定义令牌(Token)
        /// </summary>
        public static readonly string Token = ConfigHelper.GetAppSetting("WeChatToken");
        /// <summary>
        /// 消息加解密密钥 (EncodingAESKey)，消息加解密方式：安全模式
        /// </summary>
        public static readonly string EncodingAESKey = ConfigHelper.GetAppSetting("EncodingAESKey");

        #endregion

    }
}