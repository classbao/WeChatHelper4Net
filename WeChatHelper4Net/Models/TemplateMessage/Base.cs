using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 模板消息仅用于公众号向用户发送重要的服务通知，只能用于符合其要求的服务场景中，如信用卡刷卡通知，商品购买成功通知等。不支持广告等营销类消息以及其它所有可能对用户造成骚扰的消息。
	/// </summary>
	public class Base
	{
		private string _touser = string.Empty;
		private string _template_id = string.Empty;
		private string _url = string.Empty;
		private string _topcolor = string.Empty;

		/// <summary>
		/// OPENID
		/// </summary>
		public string touser { get { return _touser; } set { _touser = value; } }
		/// <summary>
		/// 模版ID（可为空，请求接口时设置默认值）
		/// </summary>
		public string template_id { get { return _template_id; } set { _template_id = value; } }
		public string url { get { return _url; } set { _url = value; } }
		/// <summary>
		/// 例如：#FF0000（可为空，请求接口时设置默认值）
		/// </summary>
		public string topcolor { get { return !string.IsNullOrEmpty(_topcolor) ? _topcolor : "#00b7ee"; } set { _topcolor = value; } }
	}

	public class DataBase
	{
		private DataItem _first;
		private DataItem _remark;

		/// <summary>
		/// 第一行消息
		/// </summary>
		public DataItem first { get { return _first; } set { _first = value; } }
		/// <summary>
		/// 底部消息备注
		/// </summary>
		public DataItem remark { get { return _remark; } set { _remark = value; } }
	}

	/// <summary>
	/// 数据项
	/// </summary>
	public class DataItem
	{
		private string _value = string.Empty;
		private string _color = string.Empty;

		/// <summary>
		/// 消息体
		/// </summary>
		public string value { get { return _value; } set { _value = value; } }
		/// <summary>
		/// 例如：#173177（可为空，请求接口时设置默认值）
		/// </summary>
		public string color { get { return !string.IsNullOrEmpty(_color) ? _color : "#00b7ee"; } set { _color = value; } }
	}
}
