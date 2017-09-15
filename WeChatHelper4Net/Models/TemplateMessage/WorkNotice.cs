using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 待处理事项通知
	/// </summary>
	public class WorkNotice : Base
	{
		private WorkNoticeData _data;
		public WorkNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class WorkNoticeData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;

		/// <summary>
		/// 参与者
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 项目名称
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 项目内容
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
	}
}
