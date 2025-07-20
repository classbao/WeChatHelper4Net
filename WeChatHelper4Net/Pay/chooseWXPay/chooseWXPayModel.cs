/**
* 命名空间: WeChatHelper4Net.Pay.chooseWXPay
*
* 功 能： N/A
* 类 名： chooseWXPayModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 15:33:46 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Pay.chooseWXPay
{
    /// <summary>
    /// 使用JS-SDK里面的wx.chooseWXPay()发起一个微信支付请求参数模型
    /// </summary>
    public class chooseWXPayModel
    {
        /// <summary>
        /// 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// 支付签名随机串，不长于 32 位
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 支付签名
        /// </summary>
        public string paySign { get; set; }
    }
}
