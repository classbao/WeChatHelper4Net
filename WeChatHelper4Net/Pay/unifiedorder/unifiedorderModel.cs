/**
* 命名空间: WeChatHelper4Net.Pay.unifiedorder
*
* 功 能： N/A
* 类 名： unifiedorderModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 15:28:46 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using WeChatHelper4Net.Pay.baseModel;


/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Pay.unifiedorder
{
    /// <summary>
    /// 统一下单
	/// 详见文档：http://pay.weixin.qq.com/wiki/doc/api/index.php?chapter=9_1
    /// </summary>
    [DataContract(Name = "xml", Namespace = "")]
    [Serializable]
    public class unifiedorderModel: signModel
    {
        public unifiedorderModel()
        {
            device_info = "WEB";
            sign_type = "MD5";
            fee_type = "CNY";
            trade_type = "JSAPI";
        }

        /// <summary>
        /// 设备号（非必填），device_info，否，String(32)，013467007045764，终端设备号(游戏wap支付此字段必传)，终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        [XmlIgnore]
        public string device_info { get; set; }

        /// <summary>
        /// 商品描述，body，是，String(32)，Ipad mini  16G  白色，商品或支付单简要描述
        /// </summary>
        [XmlIgnore]
        public string body { get; set; }
        /// <summary>
        /// 商品详情，detail，否，String(8192)，Ipad mini  16G  白色，商品名称明细列表
        /// </summary>
        [XmlIgnore]
        public string detail { get; set; }
        /// <summary>
        /// 附加数据，attach，否，String(127)，说明，附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据
        /// </summary>
        [XmlIgnore]
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号，out_trade_no，是，String(32)，1217752501201407033233368018，商户系统内部的订单号,32个字符内、仅支持使用字母、数字、中划线-、下划线_、竖线|、星号*这些英文半角字符的组合，请勿使用汉字或全角等特殊字符。 其他说明见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 货币类型，fee_type，否，String(16)，CNY，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 总金额，total_fee，是，Int，888，订单总金额，只能为整数，详见支付金额。交易金额默认为人民币交易，接口中参数支付金额单位为【分】，参数值不能带小数。对账单中的交易金额单位为【元】。外币交易的支付金额精确到币种的最小单位，参数值不能带小数点。
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP，spbill_create_ip，是，String(16)，8.8.8.8，APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。必须传正确的用户端IP,支持ipv4、ipv6格式。
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间，time_start，否，String(14)，20091225091010，订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间，time_expire，否，String(14)，20091227091010，订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。其他详见时间规则
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// 商品标记，goods_tag，否，String(32)，WXG，商品标记，代金券或立减优惠功能的参数，说明详见代金券或立减优惠
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>
        /// 通知地址，notify_url，是，String(256)，http://www.baidu.com/，接收微信支付异步通知回调地址
        /// </summary>
        [XmlIgnore]
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型，trade_type，是，String(16)，JSAPI，取值如下：JSAPI--JSAPI支付（或小程序支付）、NATIVE--Native支付、APP--app支付，MWEB--H5支付，不同trade_type决定了调起支付的方式，请根据支付产品正确上传。MICROPAY--付款码支付，付款码支付有单独的支付接口，所以接口不需要上传，该字段在对账单中会出现
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 商品ID，product_id，否，String(32)，12235413214070356458058，trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// 用户标识，openid，否，String(128)，oUpF8uMuAJO_M2pxb1Q9zNjWeS6o，trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。
        /// </summary>
        [XmlIgnore]
        public string openid { get; set; }

        /// <summary>
        /// 电子发票入口开放标识，receipt，否，String(8)，Y，传入Y时，支付成功消息和支付详情页将出现开票入口。需要在微信支付商户平台或微信公众平台开通电子发票功能，传此字段才可生效
        /// </summary>
        public string receipt { get; set; }
        /// <summary>
        /// 是否需要分账：profit_sharing，否，String(16)，Y，Y-是，需要分账，N-否，不分账，字母要求大写，不传默认不分账
        /// </summary>
        public string profit_sharing { get; set; }
        /// <summary>
        /// 场景信息，scene_info，是，String(256)，该字段用于上报支付的场景信息,针对H5支付有以下三种场景,请根据对应场景上报,H5支付不建议在APP端使用，针对场景1，2请接入APP支付，不然可能会出现兼容性问题
        /// </summary>
        [XmlIgnore]
        public string scene_info { get; set; }

    }
}
