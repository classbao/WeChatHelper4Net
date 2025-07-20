/**
* 命名空间: WeChatHelper4Net.Pay.unifiedorder
*
* 功 能： N/A
* 类 名： unifiedorderResultModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 15:30:20 熊仔其人 xxh 4.0.30319.42000 初版
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
    /// 统一下单返回结果
    /// </summary>
    public class unifiedorderResultModel: ResultBaseModel
    {


        /// <summary>
        /// 公众账号ID，appid，是，String(32)，wx8888888888888888，调用接口提交的公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号，mch_id，是，String(32)，1900000109，调用接口提交的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号，device_info，否，String(32)，013467007045764，调用接口提交的终端设备号
        /// </summary>
        public string device_info { get; set; }


        /// <summary>
        /// 交易类型，trade_type，是，String(16)，JSAPI，调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 预支付交易会话标识，prepay_id，是，String(64)，wx201410272009395522657a690389285100，微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接，code_url，否，String(64)，URl：weixin://wxpay/s/An4baqw，trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        public string code_url { get; set; }
    }
}
