using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.Pay;

namespace WeChatHelper4Net
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class Pay
	{
		/// <summary>
		/// 微信支付统一下单接口
		/// </summary>
		/// <param name="unifiedorder">统一下单参数</param>
		/// <param name="time">当前时间</param>
		/// <returns></returns>
		public static unifiedorderResultModel unifiedorder(unifiedorderModel unifiedorder, DateTime time)
		{
			unifiedorder.out_trade_no = !string.IsNullOrWhiteSpace(unifiedorder.out_trade_no) ? unifiedorder.out_trade_no : (Privacy.PartnerID + time.ToString("yyyyMMddHHmmss") + RandomCode.createRandomCode(3, true));
			unifiedorder.fee_type = !string.IsNullOrWhiteSpace(unifiedorder.fee_type) ? unifiedorder.fee_type : "CNY";
			unifiedorder.time_start = !string.IsNullOrWhiteSpace(unifiedorder.time_start) ? unifiedorder.time_start : time.ToString("yyyyMMddHHmmss");
			unifiedorder.trade_type = !string.IsNullOrWhiteSpace(unifiedorder.trade_type) ? unifiedorder.trade_type : "JSAPI";

			unifiedorder.appid = Privacy.AppId;
			unifiedorder.mch_id = Privacy.PartnerID;
			unifiedorder.nonce_str = RandomCode.createRandomCode(16, true);
			unifiedorder.sign = "";

			Dictionary<string, string> unifiedorderParams = new Dictionary<string, string>();
			unifiedorderParams.Add("appid", unifiedorder.appid);
			unifiedorderParams.Add("mch_id", unifiedorder.mch_id);
			unifiedorderParams.Add("device_info", unifiedorder.device_info);
			unifiedorderParams.Add("nonce_str", unifiedorder.nonce_str);
			unifiedorderParams.Add("sign", unifiedorder.sign);
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

			string sign = "";
			string package = GetPackage(unifiedorderParams, out sign);

			unifiedorder.sign = sign;
			unifiedorderParams["sign"] = sign;

			/*
			 * 注：参数值用XML转义即可，CDATA标签用于说明数据不被XML解析器解析。
			 */
			string unifiedorderXml = unifiedorderParams.ToXml();
			unifiedorderXml = unifiedorderXml.Replace("<unifiedorderModel>", "<xml>").Replace("</unifiedorderModel>", "</xml>");
			string resultXml = string.Empty;
			try
			{
				/*
				 * 得到最终发送的数据：
				 */
				resultXml = HttpRequestHelper.Request("https://api.mch.weixin.qq.com/pay/unifiedorder", unifiedorderXml, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				resultXml = resultXml.Replace("<xml>", "<unifiedorderResultModel>").Replace("</xml>", "</unifiedorderResultModel>");
				unifiedorderResultModel result = WeChatHelper4Net.XmlHelper.DeSerialize<unifiedorderResultModel>(resultXml);
				return result;
			}
			catch (Exception Ex)
			{
				LogHelper.Save("Pay > unifiedorder：unifiedorderXml=" + unifiedorderXml + "，resultXml=" + resultXml, "Pay", LogType.Error, LogTime.day);
				throw Ex;
			}
		}
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
			chooseWXPay.timestamp = Common.ConvertTime(time);
			chooseWXPay.nonceStr = RandomCode.createRandomCode(16, true);
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
			string package = GetPackage(chooseWXPayParams, out sign);

			chooseWXPay.paySign = sign;
			return chooseWXPay;
		}

		/// <summary>
		/// 获取Package（2.6节）
		/// </summary>
		/// <param name="dictionarys">参数字典</param>
		/// <param name="sign">out sign</param>
		/// <returns>package的值</returns>
		public static string GetPackage(System.Collections.Generic.Dictionary<string, string> dictionarys, out string sign)
		{
			if (dictionarys == null || dictionarys.Count < 1)
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
			sign = Common.MD5Encrypt(stringA + "&key=" + Privacy.PartnerKey).ToUpper();
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
				{"appkey",Privacy.AppKey}
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
			if (signType.ToUpper() == "SHA1")
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
			if (dictionarys == null || dictionarys.Count < 1) return string.Empty;
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
				{"appkey",Privacy.AppKey}
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
			if (signType.ToUpper() == "SHA1")
				paySign = Common.SHA1Encrypt(string1);
			else
				paySign = Common.MD5Encrypt(string1);
			return paySign;
		}

		/// <summary>
		/// 微信红包支付
		/// </summary>
		/// <returns></returns>
		public static string GetBonus(string CurrentWXID, string act_id, string act_name, int money, string wishing, string remark, string nick_name, string send_name)
		{
			//构建微信红包接口参数对象
			BonusModel bonusModel = new BonusModel()
			{
				sign = "",
				mch_billno = Privacy.PartnerID + DateTime.Now.ToString("yyyyMMdd") + GetTenRandomNum(),
				mch_id = Privacy.PartnerID,
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
				nonce_str = RandomCode.createRandomCode(16, true),
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
			string package = GetPackage(singWXBonusParams, out sign);
			bonusModel.sign = sign;
			string tempxmlstr = WeChatHelper4Net.XmlHelper.Serialize(bonusModel);
			string xml = tempxmlstr.Substring(tempxmlstr.IndexOf("<sign>"));
			xml = "<xml>" + xml;
			//调用微信红包接口，支付用户红包奖金
			string resultXml = Send(xml, "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack");
			//处理接口返回数据
			resultXml = resultXml.Replace("<xml>", "<BonusResultModel>").Replace("</xml>", "</BonusResultModel>");
			return resultXml;
		}

		/// <summary>
		/// 随机生成10位数字
		/// </summary>
		/// <returns></returns>
		public static string GetTenRandomNum()
		{
			StringBuilder numS = new StringBuilder();
			Random r = new Random();
			for (int i = 0; i < 10; i++)
			{
				numS.Append(r.Next(10));
			}
			return numS.ToString();
		}

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
				X509Certificate2 cer = store.Certificates.Find(X509FindType.FindBySubjectName, "人大数媒科技(北京)有限公司", false)[0];
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
			catch (Exception exception)
			{
				LogHelper.Save(exception);
				throw exception;
			}
			string str = string.Empty;
			using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
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
			if (errors == SslPolicyErrors.None)
				return true;
			return false;
		}

		#region 单位转换&字段类型说明
		/// <summary>
		/// 交易类型
		/// </summary>
		/// <param name="trade_type">trade_type</param>
		/// <returns>中文交易类型</returns>
		public static string GetTradeTypeToChinese(string trade_type)
		{
			if (string.IsNullOrWhiteSpace(trade_type)) return string.Empty;
			switch (trade_type.Trim())
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
			if (string.IsNullOrWhiteSpace(fee_type)) return string.Empty;
			switch (fee_type.Trim())
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
			if (string.IsNullOrWhiteSpace(bank_type)) return string.Empty;
			switch (bank_type.Trim())
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
