using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.Models
{
    public class NativePayResultModel
    {
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string trade_type { get; set; }
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接trade_type=NATIVE时有返回，此url用于生成支付二维码，然后提供给用户进行扫码支付。 注意：code_url的值并非固定，使用时按照URL格式转成二维码即可。时效性为2小时
        /// </summary>
        public string code_url { get; set; }
        public string device_info { get; set; }
        public string result_code { get; set; }
        public string return_msg { get; set; }


        /// <summary>
        /// 商户订单号，out_trade_no，是，String(32)，1217752501201407033233368018，商户系统内部的订单号,32个字符内、仅支持使用字母、数字、中划线-、下划线_、竖线|、星号*这些英文半角字符的组合，请勿使用汉字或全角等特殊字符。 其他说明见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商品ID，product_id，否，String(32)，12235413214070356458058，trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string product_id { get; set; }

    }
}