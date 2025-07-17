using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models.Pay
{
	
    


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
