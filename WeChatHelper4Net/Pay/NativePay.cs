/**
* 命名空间: WeChatHelper4Net.Pay
*
* 功 能： N/A
* 类 名： NativePay
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 11:19:12 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Pay.unifiedorder;

namespace WeChatHelper4Net.Pay
{
    /// <summary>
    /// Native支付：商户系统按微信支付协议生成支付二维码，用户扫码支付。Native支付是指商户系统按微信支付协议生成支付二维码，用户再用微信“扫一扫”完成支付的模式。该模式适用于PC网站、实体店单品或订单、媒体广告支付等场景。
    /// 微信【商户平台】-【产品中心】查看（已开通）
    /// </summary>
    public class NativePay
    {
        /*
API参数名，详细说明
APPID：appid，appid是微信公众账号或开放平台APP的唯一标识，在公众平台申请公众账号或者在开放平台申请APP账号后，微信会自动分配对应的appid，用于标识该应用。可在微信公众平台-->开发-->基本配置里面查看，商户的微信支付审核通过邮件中也会包含该字段值。
微信支付商户号：mch_id，商户申请微信支付后，由微信支付分配的商户收款账号。
API密钥：key，交易过程生成签名的密钥，仅保留在商户系统和微信支付后台，不会在网络中传播。商户妥善保管该Key，切勿在网络中传输，不能在其他客户端中存储，保证key不会被泄露。商户可根据邮件提示登录微信商户平台进行设置。也可按以下路径设置：微信商户平台-->账户中心-->账户设置-->API安全-->设置API密钥
Appsecret：secret，AppSecret是APPID对应的接口密码，用于获取接口调用凭证access_token时使用。在微信支付中，先通过OAuth2.0接口获取用户openid，此openid用于微信内网页支付模式下单接口使用。可登录公众平台-->微信支付，获取AppSecret（需成为开发者且账号没有异常状态）。
         */

        /*
协议规则，更新时间：2024.11.18，商户接入微信支付，调用API必须遵循以下规则：

传输方式：为保证交易安全性，采用HTTPS传输

提交方式：采用POST方法提交

数据格式：提交和返回数据都为XML格式，根节点名为xml

字符编码：微信支付API v2仅支持UTF-8字符编码的一个子集：使用一至三个字节编码的字符。也就是说，不支持Unicode辅助平面中的四至六字节编码的字符。

签名算法：MD5/HMAC-SHA256

签名要求：请求和接收数据均需要校验签名，详细方法请参考安全规范-签名算法

证书要求：调用申请退款、撤销订单、红包接口等需要商户api证书，各api接口文档均有说明。

判断逻辑：先判断协议字段返回，再判断业务返回，最后判断交易状态

特别提示：必须严格按照API的说明进行一单一支付，一单一红包，一单一付款，在未得到支付系统明确的回复之前不要换单，防止重复支付或者重复付款
         */

        /// <summary>
        /// Native支付：统一下单
        /// </summary>
        /// <param name="unifiedorder"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static unifiedorderResultModel unifiedorder(unifiedorderModel unifiedorder, DateTime time)
        {
            unifiedorder.appid = Privacy.AppId;
            unifiedorder.mch_id = Privacy.WeChatPay_PartnerID;
            unifiedorder.nonce_str = WeChatSignature.GuidWithoutHyphens();
            unifiedorder.sign = "";
            unifiedorder.sign_type = "MD5";

            //unifiedorder.device_info = "WEB";
            unifiedorder.out_trade_no = !string.IsNullOrWhiteSpace(unifiedorder.out_trade_no) ? unifiedorder.out_trade_no : PayUtil.GenerateTransactionID(Privacy.WeChatPay_PartnerID);
            unifiedorder.time_start = !string.IsNullOrWhiteSpace(unifiedorder.time_start) ? unifiedorder.time_start : time.ToString("yyyyMMddHHmmss");
            unifiedorder.trade_type = "NATIVE";
            if(string.IsNullOrWhiteSpace(unifiedorder.product_id))
                throw new ArgumentNullException(nameof(unifiedorder.product_id));


            Dictionary<string, string> unifiedorderParams = new Dictionary<string, string>();
            unifiedorderParams.Add("appid", unifiedorder.appid);
            unifiedorderParams.Add("mch_id", unifiedorder.mch_id);
            unifiedorderParams.Add("device_info", unifiedorder.device_info);
            unifiedorderParams.Add("nonce_str", unifiedorder.nonce_str);
            unifiedorderParams.Add("sign", unifiedorder.sign);
            unifiedorderParams.Add("sign_type", unifiedorder.sign_type);
            unifiedorderParams.Add("body", unifiedorder.body);
            unifiedorderParams.Add("detail", unifiedorder.detail);
            unifiedorderParams.Add("attach", unifiedorder.attach);
            unifiedorderParams.Add("out_trade_no", unifiedorder.out_trade_no);
            unifiedorderParams.Add("fee_type", unifiedorder.fee_type);
            unifiedorderParams.Add("total_fee", unifiedorder.total_fee.ToString());
            unifiedorderParams.Add("spbill_create_ip", unifiedorder.spbill_create_ip);
            unifiedorderParams.Add("time_start", unifiedorder.time_start);
            unifiedorderParams.Add("time_expire", unifiedorder.time_expire);
            unifiedorderParams.Add("goods_tag", unifiedorder.goods_tag);
            unifiedorderParams.Add("notify_url", unifiedorder.notify_url);
            unifiedorderParams.Add("trade_type", unifiedorder.trade_type);
            unifiedorderParams.Add("product_id", unifiedorder.product_id);
            unifiedorderParams.Add("openid", unifiedorder.openid);
            unifiedorderParams.Add("receipt", unifiedorder.receipt);
            unifiedorderParams.Add("profit_sharing", unifiedorder.profit_sharing);
            unifiedorderParams.Add("scene_info", unifiedorder.scene_info);


            unifiedorder.sign = WeChatSignature.Generate(unifiedorderParams, Privacy.WeChatPay_PartnerKey, unifiedorder.sign_type);
            unifiedorderParams["sign"] = unifiedorder.sign;

            /*
			 * 注：参数值用XML转义即可，CDATA标签用于说明数据不被XML解析器解析。
			 */
            string unifiedorderXml = unifiedorderParams.ToXml("xml");
            LogHelper.Save($"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口参数已准备好。", nameof(NativePay) + "_", LogType.Report, LogTime.day);

            try
            {
                /*
				 * 得到最终发送的数据：
<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg><result_code><![CDATA[SUCCESS]]></result_code><mch_id><![CDATA[1718930133]]></mch_id><appid><![CDATA[wxbb667fa941e06b3a]]></appid><device_info><![CDATA[WEB]]></device_info><nonce_str><![CDATA[4tFGmu15vvEFoPyK]]></nonce_str><sign><![CDATA[99D82EEA0D64485E9F95F2BFF2FCC235]]></sign><prepay_id><![CDATA[wx13204000885537f50ed7f6c42b5e750000]]></prepay_id><trade_type><![CDATA[NATIVE]]></trade_type><code_url><![CDATA[weixin://wxpay/bizpayurl?pr=4v3fQYUz3]]></code_url></xml>
				 */
                XmlDocument resultXml = XmlPostHelper.Post("https://api.mch.weixin.qq.com/pay/unifiedorder", unifiedorderXml, false);
                LogHelper.Save($"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口响应结果：resultXml={resultXml.OuterXml}", nameof(NativePay) + "_", LogType.Report, LogTime.day);
                unifiedorderResultModel result = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel>(resultXml.OuterXml.Replace("<xml>", "<unifiedorderResultModel>").Replace("</xml>", "</unifiedorderResultModel>"));

                return result;
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex, $"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口响应结果处理异常", nameof(NativePay), LogTime.day);
                throw Ex;
            }
        }



    }
}
