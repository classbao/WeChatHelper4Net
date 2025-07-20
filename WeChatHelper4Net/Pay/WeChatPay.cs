/**
* 命名空间: WeChatHelper4Net.Pay
*
* 功 能： N/A
* 类 名： WeChatPay
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/12 19:44:16 熊仔其人 xxh 4.0.30319.42000 初版
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
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.Pay;
using WeChatHelper4Net.Pay.chooseWXPay;
using WeChatHelper4Net.Pay.notify;
using WeChatHelper4Net.Pay.unifiedorder;

namespace WeChatHelper4Net.Pay
{
    /// <summary>
    /// JSAPI支付：商户已有H5商城网站，用户通过消息或扫描二维码在微信内打开网页时，可以调用微信支付完成下单购买的流程。
    /// </summary>
    public class WeChatPay
    {
        /// <summary>
		/// 统一下单：微信支付统一下单接口
		/// </summary>
		/// <param name="unifiedorder">统一下单参数</param>
		/// <param name="time">当前时间</param>
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
            unifiedorder.trade_type = "JSAPI";
            if(string.IsNullOrWhiteSpace(unifiedorder.openid))
                throw new ArgumentNullException(nameof(unifiedorder.openid));


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
            LogHelper.Save($"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口参数已准备好。", nameof(WeChatPay) + "_", LogType.Report, LogTime.day);

            //string resultXml = string.Empty;
            try
            {
                /*
				 * 得到最终发送的数据：
				 */
                XmlDocument resultXml = XmlPostHelper.Post("https://api.mch.weixin.qq.com/pay/unifiedorder", unifiedorderXml, false);
                //LogHelper.Save($"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口响应结果：resultXml={resultXml.OuterXml}", nameof(WeChatPay) + "_", LogType.Report, LogTime.day);
                unifiedorderResultModel result = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel>(resultXml.OuterXml.Replace("<xml>", "<unifiedorderResultModel>").Replace("</xml>", "</unifiedorderResultModel>"));

                return result;
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex, $"{nameof(unifiedorder)}，time={time.ToString()}，unifiedorderXml={unifiedorderXml}，微信支付统一下单接口响应结果处理异常", nameof(WeChatPay), LogTime.day);
                throw Ex;
            }
        }


        #region 微信支付回调接口，通知地址（notify_url）回调数据

        /// <summary>
        /// 微信支付回调接口，通知地址（notify_url）回调，签名验证
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static bool VerifyWeChatSignature(XmlDocument xmlDoc, string signType = "MD5")
        {
            // 获取微信支付API密钥（从配置中读取）
            string apiKey = Privacy.WeChatPay_PartnerKey;

            // 提取参数并验证签名
            var parameters = new Dictionary<string, string>();
            foreach(XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                if(node.Name != "sign") // 签名本身不参与签名验证
                {
                    parameters.Add(node.Name, node.InnerText);
                }
            }

            string receivedSign = xmlDoc.SelectSingleNode("xml/sign")?.InnerText;
            return WeChatSignature.Verify(parameters, apiKey, receivedSign, signType);
        }

        /// <summary>
        /// 微信支付回调接口，通知地址（notify_url）回调，数据序列化（转换为对象）
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static notifyModel unifiedorderNotifyResult(System.Xml.XmlDocument xmlDoc)
        {
            return new notifyModel
            {
                appid = xmlDoc.SelectSingleNode("xml/appid")?.InnerText,
                attach = xmlDoc.SelectSingleNode("xml/attach")?.InnerText,
                bank_type = xmlDoc.SelectSingleNode("xml/bank_type")?.InnerText,
                cash_fee = xmlDoc.SelectSingleNode("xml/cash_fee")?.InnerText,
                device_info = xmlDoc.SelectSingleNode("xml/device_info")?.InnerText,
                fee_type = xmlDoc.SelectSingleNode("xml/fee_type")?.InnerText,
                is_subscribe = xmlDoc.SelectSingleNode("xml/is_subscribe")?.InnerText,
                mch_id = xmlDoc.SelectSingleNode("xml/mch_id")?.InnerText,
                nonce_str = xmlDoc.SelectSingleNode("xml/nonce_str")?.InnerText,
                openid = xmlDoc.SelectSingleNode("xml/openid")?.InnerText,
                out_trade_no = xmlDoc.SelectSingleNode("xml/out_trade_no")?.InnerText,
                result_code = xmlDoc.SelectSingleNode("xml/result_code")?.InnerText,
                return_code = xmlDoc.SelectSingleNode("xml/return_code")?.InnerText,
                return_msg = xmlDoc.SelectSingleNode("xml/return_msg")?.InnerText,
                err_code = xmlDoc.SelectSingleNode("xml/err_code")?.InnerText,
                err_code_des = xmlDoc.SelectSingleNode("xml/err_code_des")?.InnerText,
                sign = xmlDoc.SelectSingleNode("xml/sign")?.InnerText,
                time_end = xmlDoc.SelectSingleNode("xml/time_end")?.InnerText,
                total_fee = int.Parse(xmlDoc.SelectSingleNode("xml/total_fee")?.InnerText ?? "0"),
                trade_type = xmlDoc.SelectSingleNode("xml/trade_type")?.InnerText,
                transaction_id = xmlDoc.SelectSingleNode("xml/transaction_id")?.InnerText,
            };
        }

        /// <summary>
        /// 生成FAIL状态的Xml响应
        /// </summary>
        /// <returns></returns>
        public static string GenerateSuccessXml()
        {
            return @"<xml>
                  <return_code><![CDATA[SUCCESS]]></return_code>
                  <return_msg><![CDATA[OK]]></return_msg>
                </xml>";
        }
        /// <summary>
        /// 生成SUCCESS状态的Xml响应
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GenerateErrorXml(string message)
        {
            return $@"<xml>
                  <return_code><![CDATA[FAIL]]></return_code>
                  <return_msg><![CDATA[{message}]]></return_msg>
                </xml>";
        }

        #endregion



        /// <summary>
        /// JS-SDK里面的wx.chooseWXPay()发起一个微信支付请求参数
        /// </summary>
        /// <param name="prepay_id">统一支付接口返回的prepay_id参数值</param>
        /// <param name="time">当前时间</param>
        /// <returns></returns>
        public static chooseWXPayModel chooseWXPay(string prepay_id, DateTime time)
        {
            /*
			 * paySign 采用统一的微信支付 Sign 签名生成方法，注意这里 appId 也要参与签名，appId 与 config 中传入的 appId 一致。
			 * 即最后参与签名的参数有appId, timeStamp, nonceStr, package, signType。
			 */
            chooseWXPayModel chooseWXPay = new chooseWXPayModel();
            chooseWXPay.timestamp =TimestampHelper.ConvertTime(time);
            chooseWXPay.nonceStr = RandomCode.GenerateRandomCode(16);
            chooseWXPay.package = "prepay_id=" + prepay_id;
            chooseWXPay.signType = "MD5";
            chooseWXPay.paySign = "";

            Dictionary<string, string> chooseWXPayParams = new Dictionary<string, string>();
            chooseWXPayParams.Add("appId", Privacy.AppId);
            chooseWXPayParams.Add("timeStamp", chooseWXPay.timestamp.ToString());
            chooseWXPayParams.Add("nonceStr", chooseWXPay.nonceStr);
            chooseWXPayParams.Add("package", chooseWXPay.package);
            chooseWXPayParams.Add("signType", chooseWXPay.signType);

            string sign = "";
            string package = PayUtil.GetPackage(chooseWXPayParams, out sign);

            chooseWXPay.paySign = sign;
            return chooseWXPay;
        }

        /// <summary>
        /// 微信红包支付
        /// </summary>
        /// <returns></returns>
        public static XmlDocument GetBonus(string CurrentWXID, string act_id, string act_name, int money, string wishing, string remark, string nick_name, string send_name)
        {
            //构建微信红包接口参数对象
            BonusModel bonusModel = new BonusModel()
            {
                sign = "",
                mch_billno = Privacy.WeChatPay_PartnerID + DateTime.Now.ToString("yyyyMMdd") + RandomCode.GenerateRandomNum(10),
                mch_id = Privacy.WeChatPay_PartnerID,
                wxappid = Privacy.AppId,
                nick_name = nick_name,
                send_name = send_name,
                re_openid = CurrentWXID,
                total_amount = money,
                min_value = money,
                max_value = money,
                total_num = 1,
                wishing = wishing,
                client_ip = System.Configuration.ConfigurationManager.AppSettings["serverIP"].ToString(), //"219.234.83.88",
                act_name = act_name,
                act_id = act_id,
                remark = remark,
                nonce_str = RandomCode.GenerateRandomCode(16),
            };
            Dictionary<string, string> singWXBonusParams = new Dictionary<string, string>();
            singWXBonusParams.Add("nonce_str", bonusModel.nonce_str);
            singWXBonusParams.Add("mch_billno", bonusModel.mch_billno);
            singWXBonusParams.Add("mch_id", bonusModel.mch_id);
            singWXBonusParams.Add("wxappid", bonusModel.wxappid);
            singWXBonusParams.Add("nick_name", bonusModel.nick_name);
            singWXBonusParams.Add("send_name", bonusModel.send_name);
            singWXBonusParams.Add("re_openid", bonusModel.re_openid);
            singWXBonusParams.Add("total_amount", bonusModel.total_amount.ToString());
            singWXBonusParams.Add("min_value", bonusModel.min_value.ToString());
            singWXBonusParams.Add("max_value", bonusModel.max_value.ToString());
            singWXBonusParams.Add("total_num", bonusModel.total_num.ToString());
            singWXBonusParams.Add("wishing", bonusModel.wishing);
            singWXBonusParams.Add("client_ip", bonusModel.client_ip);
            singWXBonusParams.Add("act_name", bonusModel.act_name);
            singWXBonusParams.Add("remark", bonusModel.remark);
            singWXBonusParams.Add("act_id", bonusModel.act_id);
            string sign = "";
            string package = PayUtil.GetPackage(singWXBonusParams, out sign);
            bonusModel.sign = sign;
            string tempxmlstr = WeChatHelper4Net.XmlHelper.Serialize(bonusModel);
            string xml = tempxmlstr.Substring(tempxmlstr.IndexOf("<sign>"));
            xml = "<xml>" + xml;
            //调用微信红包接口，支付用户红包奖金
            XmlDocument resultXml = XmlPostHelper.Post("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack", xml, true);
            //处理接口返回数据
            //resultXml = resultXml.Replace("<xml>", "<BonusResultModel>").Replace("</xml>", "</BonusResultModel>");
            return resultXml;
        }

    }
}
