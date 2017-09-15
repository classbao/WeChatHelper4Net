using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models.Pay
{
	/// <summary>
	/// 微信支付相关模型
	/// </summary>
	public class PayModel
	{
	}

	/// <summary>
	/// 统一下单
	/// 详见文档：http://pay.weixin.qq.com/wiki/doc/api/index.php?chapter=9_1
	/// </summary>
	public class unifiedorderModel
	{
		/// <summary>
		/// 公众账号ID，appid，是，String(32)，wx8888888888888888，微信分配的公众账号ID
		/// </summary>
		public string appid { get; set; }
		/// <summary>
		/// 商户号，mch_id，是，String(32)，1900000109，微信支付分配的商户号
		/// </summary>
		public string mch_id { get; set; }
		/// <summary>
		/// 设备号，device_info，否，String(32)，013467007045764，终端设备号(游戏wap支付此字段必传)，终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
		/// </summary>
		public string device_info { get; set; }
		/// <summary>
		/// 随机字符串，nonce_str，是，String(32)，5K8264ILTKCH16CQ2502SI8ZNMTM67VS，随机字符串，不长于32位。
		/// </summary>
		public string nonce_str { get; set; }
		/// <summary>
		/// 签名，sign，是，String(32)，C380BEC2BFD727A4B6845133519F3AD6
		/// </summary>
		public string sign { get; set; }
		/// <summary>
		/// 商品描述，body，是，String(32)，Ipad mini  16G  白色，商品或支付单简要描述
		/// </summary>
		public string body { get; set; }
		/// <summary>
		/// 商品详情，detail，否，String(8192)，Ipad mini  16G  白色，商品名称明细列表
		/// </summary>
		public string detail { get; set; }
		/// <summary>
		/// 附加数据，attach，否，String(127)，说明，附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据
		/// </summary>
		public string attach { get; set; }
		/// <summary>
		/// 商户订单号，out_trade_no，是，String(32)，1217752501201407033233368018，商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号
		/// </summary>
		public string out_trade_no { get; set; }
		/// <summary>
		/// 货币类型，fee_type，否，String(16)，CNY，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
		/// </summary>
		public string fee_type { get; set; }
		/// <summary>
		/// 总金额，total_fee，是，Int，888，订单总金额，只能为整数，详见支付金额。交易金额默认为人民币交易，接口中参数支付金额单位为【分】，参数值不能带小数。对账单中的交易金额单位为【元】。
		/// </summary>
		public int total_fee { get; set; }
		/// <summary>
		/// 终端IP，spbill_create_ip，是，String(16)，8.8.8.8，APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
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
		public string notify_url { get; set; }
		/// <summary>
		/// 交易类型，trade_type，是，String(16)，JSAPI，取值如下：JSAPI，NATIVE，APP，WAP,详细说明见参数规定
		/// </summary>
		public string trade_type { get; set; }
		/// <summary>
		/// 商品ID，product_id，否，String(32)，12235413214070356458058，trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
		/// </summary>
		public string product_id { get; set; }
		/// <summary>
		/// 用户标识，openid，否，String(128)，oUpF8uMuAJO_M2pxb1Q9zNjWeS6o，trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。。
		/// </summary>
		public string openid { get; set; }

	}

	/// <summary>
	/// 统一下单返回结果
	/// </summary>
	public class unifiedorderResultModel
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

	/// <summary>
	/// 使用JS-SDK里面的wx.chooseWXPay()发起一个微信支付请求参数模型
	/// </summary>
	public class chooseWXPayModel
	{
		/// <summary>
		/// 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
		/// </summary>
		public int timestamp { get; set; }
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

	/// <summary>
	/// 微信返回支付通知模型
	/// </summary>
	public class notifyModel
	{
		public string appid { get; set; }
		public string attach { get; set; }
		public string bank_type { get; set; }
		public string cash_fee { get; set; }
		public string device_info { get; set; }
		public string fee_type { get; set; }
		public string is_subscribe { get; set; }
		public string mch_id { get; set; }
		public string nonce_str { get; set; }
		public string openid { get; set; }
		public string out_trade_no { get; set; }
		public string result_code { get; set; }
		public string return_code { get; set; }
		public string sign { get; set; }
		public string time_end { get; set; }
		public int total_fee { get; set; }
		public string trade_type { get; set; }
		public string transaction_id { get; set; }

		//<xml>
		//	<appid>
		//		<!--[CDATA[wx7d00660ce462f8f9]]-->
		//	</appid>
		//	<attach>
		//		<!--[CDATA[附加数据]]-->
		//	</attach>
		//	<bank_type>
		//		<!--[CDATA[CMB_CREDIT]]-->
		//	</bank_type>
		//	<cash_fee>
		//		<!--[CDATA[1]]-->
		//	</cash_fee>
		//	<device_info>
		//		<!--[CDATA[WEB]]-->
		//	</device_info>
		//	<fee_type>
		//		<!--[CDATA[CNY]]-->
		//	</fee_type>
		//	<is_subscribe>
		//		<!--[CDATA[Y]]-->
		//	</is_subscribe>
		//	<mch_id>
		//		<!--[CDATA[1219538201]]-->
		//	</mch_id>
		//	<nonce_str>
		//		<!--[CDATA[E9RDuEsnysVFJ1qY]]-->
		//	</nonce_str>
		//	<openid>
		//		<!--[CDATA[o==NRtx58MS4JX9ilO_BV-VjBAGU]]-->
		//	</openid>
		//	<out_trade_no>
		//		<!--[CDATA[201504221636523252887]]-->
		//	</out_trade_no>
		//	<result_code>
		//		<!--[CDATA[SUCCESS]]-->
		//	</result_code>
		//	<return_code>
		//		<!--[CDATA[SUCCESS]]-->
		//	</return_code>
		//	<sign>
		//		<!--[CDATA[5BA340B56B781E4CE7710EC8B4988DF0]]-->
		//	</sign>
		//	<time_end>
		//		<!--[CDATA[20150422163657]]-->
		//	</time_end>
		//	<total_fee>1</total_fee>
		//	<trade_type>
		//		<!--[CDATA[JSAPI]]-->
		//	</trade_type>
		//	<transaction_id>
		//		<!--[CDATA[1009720334201504220081889158]]-->
		//	</transaction_id>
		//</xml>
	}


	/// <summary>
	/// Native（原生）支付回调商户后台获取 package
	/// 微信公众平台调用时会使用 POST 方式，推送 xml 格式的 PostData
	/// </summary>
	public class requestPackageModel
	{
		private string _OpenId = string.Empty;
		private string _AppId = string.Empty;
		private int _IsSubscribe = 0;
		private string _ProductId = string.Empty;
		private int _TimeStamp = 0;
		private string _NonceStr = string.Empty;
		private string _AppSignature = string.Empty;
		private string _SignMethod = string.Empty;

		/// <summary>
		/// 点击链接准备购买商品的用户标识
		/// </summary>
		public string OpenId { get { return _OpenId; } set { _OpenId = value; } }
		/// <summary>
		/// 公众帐号的 appid
		/// </summary>
		public string AppId { get { return _AppId; } set { _AppId = value; } }
		/// <summary>
		/// 标记用户是否订阅该公众帐号，1 为关注，0 为未关注
		/// </summary>
		public int IsSubscribe { get { return _IsSubscribe; } set { _IsSubscribe = value; } }
		/// <summary>
		/// 第三方的商品 ID 号，字符串类型，32字符以下，商户需要定义并维护自己的商品 id，这个 id与一张订单等价，微信后台凭借该 id 通过POST 商户后台获取交易必须信息
		/// </summary>
		public string ProductId { get { return _ProductId; } set { _ProductId = value; } }
		/// <summary>
		/// 时间戳
		/// </summary>
		public int TimeStamp { get { return _TimeStamp; } set { _TimeStamp = value; } }
		/// <summary>
		/// 随机串
		/// </summary>
		public string NonceStr { get { return _NonceStr; } set { _NonceStr = value; } }
		/// <summary>
		/// 参数的加密签名
		/// </summary>
		public string AppSignature { get { return _AppSignature; } set { _AppSignature = value; } }
		/// <summary>
		/// 签名方式，目前只支持“SHA1” ，该字段不参与签名
		/// </summary>
		public string SignMethod { get { return _SignMethod; } set { _SignMethod = value; } }
	}

	/// <summary>
	/// 为了返回 Package 数据，回调 URL 必须返回一个 xml 格式的返回数据
	/// </summary>
	public class responsePackageModel
	{
		private string _AppId = string.Empty;
		private string _NonceStr = string.Empty;
		private int _TimeStamp = 0;
		private string _Package = string.Empty;
		private int _RetCode = 0;
		private string _RetErrMsg = string.Empty;
		private string _SignMethod = string.Empty;
		private string _AppSignature = string.Empty;


		/// <summary>
		/// 公众帐号的 appid
		/// </summary>
		public string AppId { get { return _AppId; } set { _AppId = value; } }
		/// <summary>
		/// 随机串
		/// </summary>
		public string NonceStr { get { return _NonceStr; } set { _NonceStr = value; } }
		/// <summary>
		/// 时间戳
		/// </summary>
		public int TimeStamp { get { return _TimeStamp; } set { _TimeStamp = value; } }
		/// <summary>
		/// Package 数据
		/// </summary>
		public string Package { get { return _Package; } set { _Package = value; } }
		/// <summary>
		/// RetCode 为 0 表明正确，可以定义其他错误
		/// </summary>
		public int RetCode { get { return _RetCode; } set { _RetCode = value; } }
		/// <summary>
		/// 当定义其他错误时，可以在 RetErrMsg 中填上 UTF8 编码的错误提示信息，比如“该商品已经下架” ，客户端会直接提示出来。
		/// </summary>
		public string RetErrMsg { get { return _RetErrMsg; } set { _RetErrMsg = value; } }
		/// <summary>
		/// 签名方式，目前只支持“SHA1” ，该字段不参与签名
		/// </summary>
		public string SignMethod { get { return _SignMethod; } set { _SignMethod = value; } }
		/// <summary>
		/// 参数的加密签名
		/// </summary>
		public string AppSignature { get { return _AppSignature; } set { _AppSignature = value; } }

	}

}
