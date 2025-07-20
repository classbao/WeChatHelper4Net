using SampleWebApp.DB;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChatHelper4Net;
using WeChatHelper4Net.Models.Pay;
using WeChatHelper4Net.Pay;
using WeChatHelper4Net.Pay.notify;

namespace SampleWebApp.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        public ActionResult Index()
        {
            #region 时间戳转换
            //var t1 = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(1752389251); // 2025-07-13 14:47:31
            //var t2 = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(Convert.ToDateTime("2025-07-13 14:47:31"));

            //var t3 = WeChatHelper4Net.Extend.TimestampHelper.FromBeijingTimestamp(1752389251);
            //var t4 = WeChatHelper4Net.Extend.TimestampHelper.ToBeijingTimestamp(Convert.ToDateTime("2025-07-13 14:47:31"));

            //var t5 = WeChatHelper4Net.Extend.TimestampHelper.FromTimestamp(1752389251, Convert.ToDateTime("1970-01-01 08:00:00"));
            //var t6 = WeChatHelper4Net.Extend.TimestampHelper.ToTimestamp(Convert.ToDateTime("1970-01-01 08:00:00"), Convert.ToDateTime("2025-07-13 14:47:31"));

            //var t7 = WeChatHelper4Net.Extend.TimestampHelper.FromTimeZoneAdjustedTimestamp(1752389251, 8);
            //var t8 = WeChatHelper4Net.Extend.TimestampHelper.ToTimeZoneAdjustedTimestamp(Convert.ToDateTime("2025-07-13 14:47:31"), 8);
            #endregion

            string resultXml = "<xml><err_code><![CDATA[NOAUTH]]></err_code><err_code_des><![CDATA[商户号该产品权限未开通，请前往商户平台>产品中心检查后重试。]]></err_code_des><nonce_str><![CDATA[4dMVoEmg0waiJGzz]]></nonce_str><result_code><![CDATA[FAIL]]></result_code><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg><sign><![CDATA[3639587989A579DAC45635859E0B9265]]></sign></xml>";
            //WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel resultB = WeChatHelper4Net.XmlHelper.DeserializeWithDataContract<WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel>(resultXml);
            Dictionary<string, string> dir = WeChatHelper4Net.XmlHelper.Deserialize(resultXml, "xml");

            resultXml = "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg><result_code><![CDATA[SUCCESS]]></result_code><mch_id><![CDATA[1718930133]]></mch_id><appid><![CDATA[wxbb667fa941e06b3a]]></appid><device_info><![CDATA[WEB]]></device_info><nonce_str><![CDATA[4tFGmu15vvEFoPyK]]></nonce_str><sign><![CDATA[99D82EEA0D64485E9F95F2BFF2FCC235]]></sign><prepay_id><![CDATA[wx13204000885537f50ed7f6c42b5e750000]]></prepay_id><trade_type><![CDATA[NATIVE]]></trade_type><code_url><![CDATA[weixin://wxpay/bizpayurl?pr=4v3fQYUz3]]></code_url></xml>";

            //WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel result = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel>(resultXml.Replace("<xml>", "<unifiedorderResultModel>").Replace("</xml>", "</unifiedorderResultModel>"));
            // 检查实际反序列化的字段：
            //var properties = result.GetType().GetProperties();
            //foreach(var prop in properties)
            //{
            //    Console.WriteLine($"{prop.Name}: {prop.GetValue(result)}");
            //}

            var list = db_bill.List(0, 10);

            return View();
        }

        // http://pay.networkhand.com/Bill/WeChatNativePay?channel=wenxuebank.com&device_info=YearVIP&body=文本学习服务&attach=[xxh|123]&out_trade_no=20250713020946000000&total_fee=1&product_id=YearVIP&spbill_create_ip=127.0.0.1&notify_url=http://pay.networkhand.com/Bill/PayCallback

        /// <summary>
        /// 微信支付（Native支付，即生成收款码）
        /// </summary>
        /// <returns></returns>
        public JsonResult WeChatNativePay(string channel, string device_info, string body, string attach, string out_trade_no, int total_fee, string product_id, string spbill_create_ip, string notify_url)
        {
            JsonResult rtValue = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };

            Models.NativePayResultModel r = new Models.NativePayResultModel();

            #region 基本参数校验
            if(string.IsNullOrWhiteSpace(channel))
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(channel)}";
                rtValue.Data = r;
                return rtValue;
            }
            if(string.IsNullOrWhiteSpace(body))
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(body)}";
                rtValue.Data = r;
                return rtValue;
            }
            if(string.IsNullOrWhiteSpace(out_trade_no) || !(out_trade_no.Length >= 6 || out_trade_no.Length <= 32))
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(out_trade_no)}";
                rtValue.Data = r;
                return rtValue;
            }
            if(total_fee < 1)
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(total_fee)}";
                rtValue.Data = r;
                return rtValue;
            }
            if(string.IsNullOrWhiteSpace(product_id))
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(product_id)}";
                rtValue.Data = r;
                return rtValue;
            }
            //if(string.IsNullOrWhiteSpace(spbill_create_ip))
            //{
            //    r.result_code = "400";
            //    r.return_msg = $"Invalid parameter: {nameof(spbill_create_ip)}";
            //    rtValue.Data = r;
            //    return rtValue;
            //}
            if(string.IsNullOrWhiteSpace(notify_url) || notify_url.Length > 255)
            {
                r.result_code = "400";
                r.return_msg = $"Invalid parameter: {nameof(notify_url)}，字符串长度[1, 255]";
                rtValue.Data = r;
                return rtValue;
            }

            #endregion

            try
            {
                WeChatHelper4Net.Pay.unifiedorder.unifiedorderModel unifiedorder = new WeChatHelper4Net.Pay.unifiedorder.unifiedorderModel();

                unifiedorder.device_info = device_info;
                unifiedorder.body = body;
                unifiedorder.detail = "";
                unifiedorder.attach = attach;
                unifiedorder.out_trade_no = !string.IsNullOrWhiteSpace(out_trade_no) ? out_trade_no : PayUtil.GenerateTransactionID(WeChatHelper4Net.Common.WeChatPay_PartnerID);
                //unifiedorder.fee_type = "CNY";
                unifiedorder.total_fee = total_fee; // 订单总金额，单位为分
                unifiedorder.spbill_create_ip = !string.IsNullOrWhiteSpace(spbill_create_ip) ? spbill_create_ip : PayUtil.GetClientIpAddress();
                unifiedorder.time_start = DateTime.Now.ToString("yyyyMMddHHmmss");
                //unifiedorder.time_expire = DateTime.Now.AddHours(3).ToString("yyyyMMddHHmmss");
                //unifiedorder.goods_tag = "WXG";
                unifiedorder.notify_url = WeChatHelper4Net.Common.WeChatDomainName + "/Bill/WeChatPayCallback";

                unifiedorder.product_id = product_id;  // 商品ID
                unifiedorder.receipt = "";
                //unifiedorder.profit_sharing = "";
                //unifiedorder.scene_info = "{\"h5_info\":{\"type\":\"Wap\",\"wap_url\":\"" + WeChatHelper4Net.Common.WeChatDomainName + "\",\"wap_name\":\"" + WeChatHelper4Net.Common.WeChatName + "\"}}"; // 场景信息：3，WAP网站应用

                WeChatHelper4Net.Pay.unifiedorder.unifiedorderResultModel unifiedorderResult = NativePay.unifiedorder(unifiedorder, DateTime.Now);
                if(null != unifiedorderResult && "SUCCESS" == unifiedorderResult.return_code)
                {
                    // 通信标识：成功
                    r.appid = unifiedorderResult.appid;
                    r.mch_id = unifiedorderResult.mch_id;
                    r.trade_type = unifiedorderResult.trade_type;
                    r.prepay_id = unifiedorderResult.prepay_id;
                    r.code_url = unifiedorderResult.code_url;
                    r.device_info = unifiedorderResult.device_info;
                    r.result_code = unifiedorderResult.result_code;
                    r.return_msg = unifiedorderResult.return_msg;
                    if("SUCCESS" == unifiedorderResult.result_code && !string.IsNullOrWhiteSpace(unifiedorderResult.code_url))
                    {
                        // 业务结果：微信支付统一下单成功
                        r.out_trade_no = unifiedorder.out_trade_no;
                        r.product_id = unifiedorder.product_id;

                        // 下单成功：记录数据库
                        t_bill entity = new t_bill()
                        {
                            bill_ID = unifiedorder.out_trade_no,
                            bill_appid = unifiedorder.appid,
                            bill_mch_id = unifiedorder.mch_id,
                            bill_fee_type = unifiedorder.fee_type,
                            bill_total_fee = unifiedorder.total_fee,
                            bill_time_start = unifiedorder.time_start,
                            bill_time_expire = unifiedorder.time_expire,
                            bill_goods_tag = unifiedorder.goods_tag,
                            bill_notify_url = unifiedorder.notify_url,
                            bill_trade_type = unifiedorder.trade_type,
                            bill_product_id = unifiedorder.product_id,
                            bill_openid = unifiedorder.openid,
                            bill_attach = unifiedorder.attach,
                            bill_body = unifiedorder.body,
                            bill_device_info = unifiedorder.device_info,
                            bill_receipt = unifiedorder.receipt,
                            bill_profit_sharing = unifiedorder.profit_sharing,
                            bill_scene_info = unifiedorder.scene_info,
                            bill_spbill_create_ip = unifiedorder.spbill_create_ip,

                            bill_prepay_id= unifiedorderResult.prepay_id,
                            bill_code_url = unifiedorderResult.code_url,
                        };

                        try
                        {
                            entity.bill_source_channel = channel;
                            entity.bill_pay_channel = "WeChatNativePay";
                            entity.bill_third_notify_url = notify_url;
                            entity.bill_ctime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);

                            int rowsAffected = db_bill.Add(entity);
                        }
                        catch(Exception ex)
                        {
                            LogHelper.Save(ex, $"{nameof(WeChatNativePay)}，HttpMethod={Request.HttpMethod}，t_bill={WeChatHelper4Net.JsonHelper.Serialize(entity)}，Query={Request.Url.Query}", nameof(BillController) + "_", LogTime.day);
                            throw;
                        }


                    }
                    else
                    {
                        r.result_code = unifiedorderResult.result_code;
                        r.return_msg = $"return_msg：{unifiedorderResult.return_msg}，err_code：{unifiedorderResult.err_code}，err_code_des：{unifiedorderResult.err_code_des}，";
                    }
                }
                else
                {
                    r.result_code = unifiedorderResult.result_code;
                    r.return_msg = unifiedorderResult.return_msg;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Save(ex, $"{nameof(WeChatNativePay)}，HttpMethod={Request.HttpMethod}，Query={Request.Url.Query}", nameof(BillController) + "_", LogTime.day);
                throw;
            }

            rtValue.Data = r;
            return rtValue;
        }

        /// <summary>
        /// 微信支付回调接口
        /// 通知地址（notify_url）：接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
        /// 1，必须是公网可访问的URL：微信服务器会POST数据到这个地址
        /// 2，不能带参数：notify_url 不能包含查询字符串
        /// 3，支持POST请求：只接收POST方式的请求
        /// 4，HTTPS推荐：正式环境建议使用HTTPS
        /// 5，超时处理：微信会在5秒内未收到响应时重试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeChatPayCallback()
        {
            /*
支付完成后，微信会把相关支付结果及用户信息通过数据流的形式发送给商户，商户需要接收处理，并按文档规范返回应答。
注意：
1、同样的通知可能会多次发送给商户系统。商户系统必须能够正确处理重复的通知。
2、后台通知交互时，如果微信收到商户的应答不符合规范或超时，微信会判定本次通知失败，重新发送通知，直到成功为止（在通知一直不成功的情况下，微信总共会发起多次通知，通知频率为15s/15s/30s/3m/10m/20m/30m/30m/30m/60m/3h/3h/3h/6h/6h - 总计 24h4m）这里通知发送可能会多台服务器进行发送，且发送时间可能会在几秒内，但微信不保证通知最终一定能成功。
3、在订单状态不明或者没有收到微信支付结果通知的情况下，建议商户主动调用微信支付【查询订单API】确认订单状态。

特别提醒：
1、商户系统对于支付结果通知的内容一定要做签名验证,并校验返回的订单金额是否与商户侧的订单金额一致，防止数据泄露导致出现“假通知”，造成资金损失。
2、当收到通知进行处理时，首先检查对应业务数据的状态，判断该通知是否已经处理过，如果没有处理过再进行处理，如果处理过直接返回结果成功。在对业务数据进行状态检查和处理之前，要采用数据锁进行并发控制，以避免函数重入造成的数据混乱。
该链接是通过【统一下单API】中提交的参数notify_url设置，如果链接无法访问，商户将无法接收到微信通知。
通知url必须为直接可访问的url，不能携带参数。公网域名必须为https，如果是走专线接入，使用专线NAT IP或者私有回调域名可使用http。示例：notify_url：“https://pay.weixin.qq.com/wxpay/pay.action”
             */

            try
            {
                // 1. 读取请求数据
                string xmlData;
                using(var reader = new System.IO.StreamReader(Request.InputStream))
                {
                    xmlData = reader.ReadToEnd();
                }

                // 2. 解析XML数据
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlData);
                LogHelper.Save($"{nameof(WeChatPayCallback)}，HttpMethod={Request.HttpMethod}，ContentType={Request.ContentType}，xmlData={xmlData}， xmlDoc={xmlDoc.OuterXml}，Query={Request.Url.Query}", nameof(BillController) + "_", LogType.Report, LogTime.day);

                // 3. 验证签名
                if(!WeChatPay.VerifyWeChatSignature(xmlDoc))
                {
                    LogHelper.Save($"{nameof(WeChatPayCallback)}> !WeChatPay.VerifyWeChatSignature(xmlDoc)，签名验证失败！xmlData={xmlData}", nameof(BillController) + "_", LogType.Report, LogTime.day);
                    return Content(WeChatPay.GenerateErrorXml("签名验证失败"));
                }

                // 4. 解析支付结果
                var result = WeChatPay.unifiedorderNotifyResult(xmlDoc);

                // 5. 处理业务逻辑
                bool processSuccess = ProcessWeChatPaymentResult(result, xmlData);

                // 6. 返回响应
                return processSuccess
                    ? Content(WeChatPay.GenerateSuccessXml())
                    : Content(WeChatPay.GenerateErrorXml("处理订单失败"));
            }
            catch(Exception ex)
            {
                // 记录异常日志
                //System.Diagnostics.Trace.TraceError($"微信支付回调异常: {ex}");
                LogHelper.Save(ex, $"{nameof(WeChatPayCallback)}>，服务器异常！", nameof(BillController) + "_", LogTime.day);
                return Content(WeChatPay.GenerateErrorXml("服务器异常"));
            }
        }

        /// <summary>
        /// 处理付款结果
        /// </summary>
        /// <param name="result">支付结果通知数据</param>
        /// <param name="xmlData">支付结果通知数据（原始）</param>
        /// <returns></returns>
        private bool ProcessWeChatPaymentResult(notifyModel result, string xmlData)
        {
            // 1. 检查基本返回码
            if(result.return_code != "SUCCESS" || result.result_code != "SUCCESS")
            {
                System.Diagnostics.Trace.TraceWarning($"微信支付失败: {result.out_trade_no}, 返回码: {result.return_code}, 结果码: {result.result_code}");
                LogHelper.Save($"{nameof(ProcessWeChatPaymentResult)}>，微信支付失败: {result.out_trade_no}, 返回码: {result.return_code}, 结果码: {result.result_code}", nameof(BillController) + "_", LogType.Report, LogTime.day);
                return false;
            }

            // 2. 检查订单是否存在（根据out_trade_no查询数据库）
            var order = db_bill.GetById(result.out_trade_no);
            if(order == null)
            {
                //System.Diagnostics.Trace.TraceError($"订单不存在: {result.out_trade_no}");
                LogHelper.Save($"{nameof(ProcessWeChatPaymentResult)}>，订单不存在: {result.out_trade_no}, 返回码: {result.return_code}, 结果码: {result.result_code}", nameof(BillController) + "_", LogType.Report, LogTime.day);
                return false;
            }

            // 通知第三方业务（异步发送POST请求）
            try
            {
                string response = System.Threading.Tasks.Task.Run(() => HttpClientAsync.RequestAsync(order.bill_third_notify_url, xmlData)).Result;
            }
            catch(Exception ex)
            {
                LogHelper.Save(ex, $"{nameof(ProcessWeChatPaymentResult)}> RequestAsync 通知第三方业务（异步发送POST请求）: {result.out_trade_no}, bill_notify_transaction_id: {order.bill_notify_transaction_id}, bill_notify_time_end: {order.bill_notify_time_end}", nameof(BillController) + "_", LogTime.day);
                //throw;  // 通知第三方业务（异步发送POST请求）回调接口请求异常，先忽略。让客户自己调试开发。不影响收款
            }
            finally
            {
                LogHelper.Save($"{nameof(ProcessWeChatPaymentResult)}> RequestAsync 通知第三方业务（异步发送POST请求）: {result.out_trade_no}, bill_notify_transaction_id: {order.bill_notify_transaction_id}, bill_notify_time_end: {order.bill_notify_time_end}", nameof(BillController) + "_", LogType.Report, LogTime.day);
            }

            // 3. 检查是否已处理过
            if(!string.IsNullOrWhiteSpace(order.bill_notify_transaction_id) && !string.IsNullOrWhiteSpace(order.bill_notify_time_end))
            {
                // 订单已处理过，不再重复处理
                LogHelper.Save($"{nameof(ProcessWeChatPaymentResult)}>，订单已处理过，不再重复处理: {result.out_trade_no}, bill_notify_transaction_id: {order.bill_notify_transaction_id}, bill_notify_time_end: {order.bill_notify_time_end}", nameof(BillController) + "_", LogType.Report, LogTime.day);
                return true;
            }

            // 4. 检查金额是否匹配
            if(order.bill_total_fee != result.total_fee) // 微信金额单位为分
            {
                //System.Diagnostics.Trace.TraceError($"金额不匹配, 订单: {order.bill_total_fee}, 微信: {result.total_fee}");
                LogHelper.Save($"{nameof(ProcessWeChatPaymentResult)}>，订单金额不匹配！ {result.out_trade_no}, 订单金额: {order.bill_total_fee} {order.bill_fee_type}, 微信: {result.total_fee} {result.fee_type}", nameof(BillController) + "_", LogType.Report, LogTime.day);
                return false;
            }

            // 5. 更新订单状态
            try
            {
                order.bill_return_code = result.return_code;
                order.bill_result_code = result.result_code;
                order.bill_err_code = result.err_code;

                order.bill_notify_total_fee = result.total_fee;
                order.bill_notify_fee_type = result.fee_type;
                order.bill_notify_time_end = result.time_end;
                order.bill_notify_transaction_id = result.transaction_id;

                order.bill_notifytime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);
                order.bill_bill_mtime = order.bill_notifytime;
                int rowsAffected = db_bill.NotifyCompleteBill(order);
                if(rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Save(ex, $"{nameof(ProcessWeChatPaymentResult)}>，更新订单状态报错！ {result.out_trade_no}, 订单金额: {order.bill_total_fee} {order.bill_fee_type}, 微信: {result.total_fee} {result.fee_type}", nameof(BillController) + "_", LogTime.day);
                throw;
            }

        }


    }
}