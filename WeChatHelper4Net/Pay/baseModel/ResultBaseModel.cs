/**
* 命名空间: WeChatHelper4Net.Pay.baseModel
*
* 功 能： N/A
* 类 名： ResultBaseModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 17:54:04 熊仔其人 xxh 4.0.30319.42000 初版
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
    /// 微信支付调用API返回结果（基础字段模型）
    /// </summary>
    public class ResultBaseModel
    {
        /// <summary>
        /// 返回状态码，return_code，是，String(16)，SUCCESS，SUCCESS/FAIL，此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，return_msg，否，String(128)，签名失败，返回信息，如非空，为错误原因，签名失败，参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 随机字符串，nonce_str，是，String(32)，5K8264ILTKCH16CQ2502SI8ZNMTM67VS，微信返回的随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名，sign，是，String(32)，C380BEC2BFD727A4B6845133519F3AD6，微信返回的签名，详见签名算法
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 业务结果，result_code，是，String(16)，SUCCESS，SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码，err_code，否，String(32)，SYSTEMERROR，详细参见第6节错误列表
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述，err_code_des，否，String(128)，系统错误，错误返回的信息描述
        /// </summary>
        public string err_code_des { get; set; }

    }

    /*
<xml>
<err_code><![CDATA[NOAUTH]]></err_code>
<err_code_des><![CDATA[商户号该产品权限未开通，请前往商户平台>产品中心检查后重试。]]></err_code_des>
<nonce_str><![CDATA[4dMVoEmg0waiJGzz]]></nonce_str>
<result_code><![CDATA[FAIL]]></result_code>
<return_code><![CDATA[SUCCESS]]></return_code>
<return_msg><![CDATA[OK]]></return_msg>
<sign><![CDATA[3639587989A579DAC45635859E0B9265]]></sign>
</xml>

反序列化示例：
string resultXml = "<xml><err_code><![CDATA[NOAUTH]]></err_code><err_code_des><![CDATA[商户号该产品权限未开通，请前往商户平台>产品中心检查后重试。]]></err_code_des><nonce_str><![CDATA[4dMVoEmg0waiJGzz]]></nonce_str><result_code><![CDATA[FAIL]]></result_code><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg><sign><![CDATA[3639587989A579DAC45635859E0B9265]]></sign></xml>";
WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel resultB = WeChatHelper4Net.XmlHelper.DeserializeWithDataContract<WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel>(resultXml);
     */

}
