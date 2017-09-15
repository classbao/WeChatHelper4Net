using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 微信客户端浏览器UserAgent
	/// </summary>
	public class MicroMessengerUserAgent
	{
		private bool _IsMicroMessenger = false;
		private string _Version = string.Empty;
		private string _NetType = string.Empty;

		/// <summary>
		/// 是否微信客户端浏览器
		/// </summary>
		public bool IsMicroMessenger { get { return _IsMicroMessenger; } set { _IsMicroMessenger = value; } }
		/// <summary>
		/// 微信客户端浏览器版本号
		/// </summary>
		public string Version { get { return _Version; } set { _Version = value; } }
		/// <summary>
		/// 当前网络类型
		/// </summary>
		public string NetType { get { return _NetType; } set { _NetType = value; } }
	}
}
