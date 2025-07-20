/**
* 命名空间: WeChatHelper4Net.Pay
*
* 功 能： N/A
* 类 名： PayUtil
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/12 19:52:45 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeChatHelper4Net.Extend;

namespace WeChatHelper4Net.Pay
{
    /// <summary>
    /// 支付代码工具库（微信/财付通）
    /// </summary>
    public class PayUtil
    {

        #region 获取用户真实ip地址

        /// <summary>
        /// 获取用户真实ip地址。在C#中获取客户端IP地址需要处理多种情况，包括直接连接、代理服务器和负载均衡等情况。
        /// </summary>
        /// <returns></returns>
        public static string GetClientIpAddress()
        {
            var context = HttpContext.Current;
            if(context == null || context.Request == null)
                return string.Empty;

            // 检查代理头（负载均衡/反向代理场景）
            string ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //有代理的情况：context.Request.Headers.GetValues("X-Forwarded-For")
            //有代理的情况：context.Request.Headers.GetValues("X-Real-IP")
            if(!string.IsNullOrWhiteSpace(ip))
            {
                // X-Forwarded-For可能包含多个IP（客户端IP,代理1,代理2,...）
                string[] ipList = ip.Split(',');
                ip = ipList[0].Trim(); // 取第一个IP
            }
            else
            {
                // 直接连接的客户端IP
                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }

            // 处理IPv6本地地址
            if(ip == "::1")
                ip = "127.0.0.1";

            return ip ?? string.Empty;
        }
        #endregion

        

        /// <summary>
        /// 获取Package（2.6节）
        /// </summary>
        /// <param name="dictionarys">参数字典</param>
        /// <param name="sign">out sign</param>
        /// <returns>package的值</returns>
        [Obsolete("已经永久迁移到：WeChatSignature.Generate")]
        public static string GetPackage(System.Collections.Generic.Dictionary<string, string> dictionarys, out string sign)
        {
            if(dictionarys == null || dictionarys.Count < 1)
            {
                sign = string.Empty;
                return string.Empty;
            }
            dictionarys.Remove("key");
            /*
			 * 第一步，设所有发送或者接收到的数据为集合M，将集合M内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串stringA。
			 */
            string stringA = dictionarys.Sort().ToURLParameter();
            /*
			 * 第二步，在stringA最后拼接上key=(API密钥的值)得到stringSignTemp字符串，并对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。
			 */
            sign = Common.MD5Encrypt(stringA + "&key=" + Privacy.WeChatPay_PartnerKey).ToUpper();
            return stringA + "&sign=" + sign;
        }

        /// <summary>
        /// 支付签名（paySign）生成方法（2.7节）
        /// paySign 字段是对本次发起 JS API 的行为进行鉴权，只有通过了 paySign 鉴权，才能继续对 package 鉴权并生成预支付单。这里将定义 paySign 的生成规则。
        /// 参与 paySign 签名的字段包括：appid、timestamp、noncestr、package 以及 appkey（即paySignkey） 。这里 signType 并不参与签名。
        /// 注 意 ： 以 上 操 作 可 通 过 沙 箱 测 试 验 证 签 名 的 有 效 性 ， 沙 箱 地 址 ：http://mp.weixin.qq.com/debug/cgi-bin/readtmpl?t=pay/index
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <param name="package"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static string GetPaySign(string timestamp, string noncestr, string package, string signType = "SHA1")
        {
            /*
			 * 参与 paySign 签名的字段包括：appid、timestamp、noncestr、package 以及 appkey（即paySignkey） 。这里 signType 并不参与签名。
			 */
            Dictionary<string, string> paySignParams = new Dictionary<string, string>()
            {
                {"appid",Common.AppId},
                {"timestamp",timestamp},
                {"noncestr",noncestr},
                {"package",package},
                {"appkey",Privacy.WeChatPay_SignKey}
            };
            /*
			 * a.对所有待签名参数按照字段名的 ASCII 码从小到大排序（字典序）后，
			 * 使用 URL 键值对的格式（即 key1=value1&key2=value2…）拼接成字符串 string1。
			 * 这里需要注意的是所有参数名均为小写字符，例如 appId 在排序后字符串则为 appid；
			 */
            string string1 = paySignParams.Sort().ToURLParameter();
            /*
			 * b.对 string1 作签名算法， 字段名和字段值都采用原始值 （此时 package 的 value 就对应了使用 2.6 中描述的方式生成的 package），不进行 URL 转义。
			 * 具体签名算法为 paySign = SHA1(string)。
			 */
            string paySign = string.Empty;
            if(signType.ToUpper() == "SHA1")
                paySign = Common.SHA1Encrypt(string1);
            else
                paySign = Common.MD5Encrypt(string1);
            return paySign;
        }

        /// <summary>
        /// 微信反馈的支付通知参数的签名
        /// 后台通知通过请求中的 notify_url 进行，采用 POST 机制。返回通知中的参数一致，URL包含如下内容参数
        /// </summary>
        /// <param name="dictionarys"></param>
        /// <returns></returns>
        public static string GetNotifySign(System.Collections.Generic.Dictionary<string, string> dictionarys)
        {
            if(dictionarys == null || dictionarys.Count < 1) return string.Empty;
            dictionarys.Remove("sign");

            string sign = string.Empty;
            string package = GetPackage(dictionarys, out sign);

            return sign;
        }

        /// <summary>
        /// 微信反馈的支付通知参数的支付签名
        /// AppSignature 依然是根据 2.7 节支付签名 （paySign） 签名方式生成， 参与签名的字段为：appid、appkey、timestamp、noncestr、openid、issubscribe。
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <param name="openid"></param>
        /// <param name="issubscribe"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static string GetNotifyAppSignature(string timestamp, string noncestr, string openid, string issubscribe, string signType = "SHA1")
        {
            /*
			 * 参与 paySign 签名的字段包括：appid、timestamp、noncestr、package 以及 appkey（即paySignkey） 。这里 signType 并不参与签名。
			 */
            Dictionary<string, string> paySignParams = new Dictionary<string, string>()
            {
                {"appid",Common.AppId},
                {"timestamp",timestamp},
                {"noncestr",noncestr},
                {"openid",openid},
                {"issubscribe",issubscribe},
                {"appkey",Privacy.WeChatPay_SignKey}
            };
            /*
			 * a.对所有待签名参数按照字段名的 ASCII 码从小到大排序（字典序）后，
			 * 使用 URL 键值对的格式（即 key1=value1&key2=value2…）拼接成字符串 string1。
			 * 这里需要注意的是所有参数名均为小写字符，例如 appId 在排序后字符串则为 appid；
			 */
            string string1 = paySignParams.Sort().ToURLParameter();
            /*
			 * b.对 string1 作签名算法， 字段名和字段值都采用原始值 （此时 package 的 value 就对应了使用 2.6 中描述的方式生成的 package），不进行 URL 转义。
			 * 具体签名算法为 paySign = SHA1(string)。
			 */
            string paySign = string.Empty;
            if(signType.ToUpper() == "SHA1")
                paySign = Common.SHA1Encrypt(string1);
            else
                paySign = Common.MD5Encrypt(string1);
            return paySign;
        }

        #region 生成交易单号（交易ID）
        /// <summary>
        /// 生成交易单号（交易ID）
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GenerateTransactionID(string prefix)
        {
            return string.Format("{0}{1}{2}", prefix, TimestampHelper.ConvertTime(DateTime.Now), RandomCode.GenerateRandomNum(10));
        }

        #endregion

        /*
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="data">发送拼接的参数</param>
        /// <param name="url">要发送到的链接地址</param>
        /// <returns>返回xml</returns>
        public static string Send(string data, string url)
        {
            return Send(Encoding.GetEncoding("UTF-8").GetBytes(data), url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Send(byte[] data, string url)
        {
            Stream responseStream;
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2 cer = store.Certificates.Find(X509FindType.FindBySubjectName, "xxx有限公司", false)[0];
                //string cert = ConfigurationManager.AppSettings["CertPath"].Trim();//证书存放的地址
                //string password = Privacy.PartnerID;//证书密码 即商户号
                //X509Certificate cer = new X509Certificate(cert, password);
                //#region 该部分是关键，若没有该部分则在IIS下会报 CA证书出错
                //X509Certificate2 certificate = new X509Certificate2(cert, password);
                //X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //store.Open(OpenFlags.ReadWrite);
                //store.Remove(certificate);   //可省略
                //store.Add(certificate);
                //store.Close();
                //#endregion

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ClientCertificates.Add(cer);
                request.Method = "POST";
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch(Exception exception)
            {
                LogHelper.Save(exception);
                throw exception;
            }
            string str = string.Empty;
            using(StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if(errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        */

        #region 单位转换&字段类型说明
        /// <summary>
        /// 交易类型
        /// </summary>
        /// <param name="trade_type">trade_type</param>
        /// <returns>中文交易类型</returns>
        public static string GetTradeTypeToChinese(string trade_type)
        {
            if(string.IsNullOrWhiteSpace(trade_type)) return string.Empty;
            switch(trade_type.Trim())
            {
                case "JSAPI": return "公众号支付";
                case "NATIVE": return "原生扫码支付";
                case "APP": return "app支付";
                case "WAP": return "手机浏览器H5支付";
                case "MICROPAY": return "刷卡支付";
                default: return trade_type;
            }
        }
        /// <summary>
        /// 货币类型
        /// </summary>
        /// <param name="fee_type">货币类型</param>
        /// <returns>中文货币类型</returns>
        public static string GetFeeTypeToChinese(string fee_type)
        {
            if(string.IsNullOrWhiteSpace(fee_type)) return string.Empty;
            switch(fee_type.Trim())
            {
                case "CNY": return "人民币";
                default: return fee_type;
            }
        }
        /// <summary>
        /// 付款银行
        /// </summary>
        /// <param name="bank_type">付款银行</param>
        /// <returns>中文付款银行</returns>
        public static string GetBankTypeToChinese(string bank_type)
        {
            if(string.IsNullOrWhiteSpace(bank_type)) return string.Empty;
            switch(bank_type.Trim())
            {
                case "ICBC_DEBIT": return "工商银行（借记卡）";
                case "ICBC_CREDIT": return "工商银行（信用卡）";
                case "ABC_DEBIT": return "农业银行（借记卡）";
                case "ABC_CREDIT": return "农业银行 （信用卡）";
                case "PSBC_DEBIT": return "邮政储蓄（借记卡）";
                case "PSBC_CREDIT": return "邮政储蓄 （信用卡）";
                case "CCB_DEBIT": return "建设银行（借记卡）";
                case "CCB_CREDIT": return "建设银行 （信用卡）";
                case "CMB_DEBIT": return "招商银行（借记卡）";
                case "CMB_CREDIT": return "招商银行（信用卡）";
                case "COMM_DEBIT": return "交通银行（借记卡）";
                case "BOC_CREDIT": return "中国银行（信用卡）";
                case "SPDB_DEBIT": return "浦发银行（借记卡）";
                case "SPDB_CREDIT": return "浦发银行 （信用卡）";
                case "GDB_DEBIT": return "广发银行（借记卡）";
                case "GDB_CREDIT": return "广发银行（信用卡）";
                case "CMBC_DEBIT": return "民生银行（借记卡）";
                case "CMBC_CREDIT": return "民生银行（信用卡）";
                case "PAB_DEBIT": return "平安银行（借记卡）";
                case "PAB_CREDIT": return "平安银行（信用卡）";
                case "CEB_DEBIT": return "光大银行（借记卡）";
                case "CEB_CREDIT": return "光大银行（信用卡）";
                case "CIB_DEBIT": return "兴业银行 （借记卡）";
                case "CIB_CREDIT": return "兴业银行（信用卡）";
                case "CITIC_DEBIT": return "中信银行（借记卡）";
                case "CITIC_CREDIT": return "中信银行（信用卡）";
                case "SDB_CREDIT": return "深发银行（信用卡）";
                case "BOSH_DEBIT": return "上海银行（借记卡）";
                case "BOSH_CREDIT": return "上海银行 （信用卡）";
                case "CRB_DEBIT": return "华润银行（借记卡）";
                case "HZB_DEBIT": return "杭州银行（借记卡）";
                case "HZB_CREDIT": return "杭州银行（信用卡）";
                case "BSB_DEBIT": return "包商银行（借记卡）";
                case "BSB_CREDIT": return "包商银行 （信用卡）";
                case "CQB_DEBIT": return "重庆银行（借记卡）";
                case "SDEB_DEBIT": return "顺德农商行 （借记卡）";
                case "SZRCB_DEBIT": return "深圳农商银行（借记卡）";
                case "HRBB_DEBIT": return "哈尔滨银行（借记卡）";
                case "BOCD_DEBIT": return "成都银行（借记卡）";
                case "GDNYB_DEBIT": return "南粤银行 （借记卡）";
                case "GDNYB_CREDIT": return "南粤银行 （信用卡）";
                case "GZCB_CREDIT": return "广州银行（信用卡）";
                case "JSB_DEBIT": return "江苏银行（借记卡）";
                case "JSB_CREDIT": return "江苏银行（信用卡）";
                case "NBCB_DEBIT": return "宁波银行（借记卡）";
                case "NBCB_CREDIT": return "宁波银行（信用卡）";
                case "NJCB_DEBIT": return "南京银行（借记卡）";
                case "QDCCB_DEBIT": return "青岛银行（借记卡）";
                case "ZJTLCB_DEBIT": return "浙江泰隆银行（借记卡）";
                case "XAB_DEBIT": return "西安银行（借记卡）";
                case "CSRCB_DEBIT": return "常熟农商银行 （借记卡）";
                case "QLB_DEBIT": return "齐鲁银行（借记卡）";
                case "LJB_DEBIT": return "龙江银行（借记卡）";
                case "HXB_DEBIT": return "华夏银行（借记卡）";
                case "CS_DEBIT": return "测试银行借记卡快捷支付 （借记卡）";
                case "AE_CREDIT": return "AE （信用卡）";
                case "JCB_CREDIT": return "JCB （信用卡）";
                case "MASTERCARD_CREDIT": return "MASTERCARD （信用卡）";
                case "VISA_CREDIT": return "VISA （信用卡）";
                default: return bank_type;
            }
        }
        #endregion



    }
}
