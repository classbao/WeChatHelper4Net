using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 客户留言通知
	/// </summary>
	public class CustomerMessageNotice : Base
	{
		private CustomerMessageNoticeData _data;
		public CustomerMessageNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class CustomerMessageNoticeData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;

		/// <summary>
		/// 留言内容
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 留言时间
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
	}
}
