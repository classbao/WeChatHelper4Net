using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 最新学术活动通知
	/// </summary>
	public class DynamicNotice : Base
	{
		private DynamicNoticeData _data;
		public DynamicNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class DynamicNoticeData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;
		private DataItem _keyword4;
		private DataItem _keyword5;

		/// <summary>
		/// 主题
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 主讲人
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 时间
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
		/// <summary>
		/// 地点
		/// </summary>
		public DataItem keyword4 { get { return _keyword4; } set { _keyword4 = value; } }
		/// <summary>
		/// 简要
		/// </summary>
		public DataItem keyword5 { get { return _keyword5; } set { _keyword5 = value; } }
	}
}
