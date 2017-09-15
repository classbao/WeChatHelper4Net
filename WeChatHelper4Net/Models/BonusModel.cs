using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeChatHelper4Net.Models
{
    /// <summary>
    /// 微信红包接口参数模型
    /// </summary>
    [XmlRoot("xml")]
    public class BonusModel
    {
        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement(ElementName = "sign")]
        public string sign { get; set; }
        /// <summary>
        /// 商户订单号:mch_id+yyyymmdd+10位一天内不能重复的数字。
        /// </summary>
        [XmlElement(ElementName = "mch_billno")]
        public string mch_billno { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        [XmlElement(ElementName = "mch_id")]
        public string mch_id { get; set; }
        /// <summary>
        /// 公众账号appid
        /// </summary>
        [XmlElement(ElementName = "wxappid")]
        public string wxappid { get; set; }
        /// <summary>
        /// 提供方名称
        /// </summary>
        [XmlElement(ElementName = "nick_name")]
        public string nick_name { get; set; }
        /// <summary>
        /// 商户名称(红包发送者名称)
        /// </summary>
        [XmlElement(ElementName = "send_name")]
        public string send_name { get; set; }
        /// <summary>
        /// 接受收红包的用户(用户在wxappid下的openid)
        /// </summary>
        [XmlElement(ElementName = "re_openid")]
        public string re_openid { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        [XmlElement(ElementName = "total_amount")]
        public int total_amount { get; set; }
        /// <summary>
        /// 最小红包金额
        /// </summary>
        [XmlElement(ElementName = "min_value")]
        public int min_value { get; set; }
        /// <summary>
        /// 最大红包金额
        /// </summary>
        [XmlElement(ElementName = "max_value")]
        public int max_value { get; set; }
        /// <summary>
        /// 红包发放总人数
        /// </summary>
        [XmlElement(ElementName = "total_num")]
        public int total_num { get; set; }
        /// <summary>
        /// 红包祝福语
        /// </summary>
        [XmlElement(ElementName = "wishing")]
        public string wishing { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        [XmlElement(ElementName = "client_ip")]
        public string client_ip { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        [XmlElement(ElementName = "act_name")]
        public string act_name { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [XmlElement(ElementName = "act_id")]
        public string act_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [XmlElement(ElementName = "remark")]
        public string remark { get; set; }
        /// <summary>
        /// 商户logo的url(非必填)
        /// </summary>
        [XmlElement(ElementName = "logo_imgurl")]
        public string logo_imgurl { get; set; }
        /// <summary>
        /// 分享的文案(非必填)
        /// </summary>
        [XmlElement(ElementName = "share_content")]
        public string share_content { get; set; }
        /// <summary>
        /// 分享的链接(非必填)
        /// </summary>
        [XmlElement(ElementName = "share_url")]
        public string share_url { get; set; }
        /// <summary>
        /// 分享的图片地址(非必填)
        /// </summary>
        [XmlElement(ElementName = "share_imgurl")]
        public string share_imgurl { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        [XmlElement(ElementName = "nonce_str")]
        public string nonce_str { get; set; }
    }
}
