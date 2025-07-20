/**
* 命名空间: WeChatHelper4Net.Pay.baseModel
*
* 功 能： N/A
* 类 名： signModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 18:04:18 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Runtime.Serialization;

namespace WeChatHelper4Net.Pay.baseModel
{
    /// <summary>
    /// 微信支付调用API（生成签名必须字段模型）
    /// </summary>
    [DataContract(Name = "xml", Namespace = "")]
    [Serializable]
    public class signModel
    {
        /// <summary>
        /// 公众账号ID，appid，是，String(32)，wx8888888888888888，微信分配的公众账号ID
        /// </summary>
        [DataMember]
        public string appid { get; set; }
        /// <summary>
        /// 商户号，mch_id，是，String(32)，1900000109，微信支付分配的商户号
        /// </summary>
        [DataMember]
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串，nonce_str，是，String(32)，5K8264ILTKCH16CQ2502SI8ZNMTM67VS，随机字符串，不长于32位。
        /// </summary>
        [DataMember]
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名，sign，是，String(32)，C380BEC2BFD727A4B6845133519F3AD6
        /// </summary>
        [DataMember]
        public string sign { get; set; }
        /// <summary>
        /// 签名类型（非必填），sign_type，否，String(32)，HMAC-SHA256，签名类型，目前支持HMAC-SHA256和MD5，默认为MD5
        /// </summary>
        [DataMember]
        public string sign_type { get; set; }

    }
}
