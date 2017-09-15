using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 活动报名成功通知
	/// </summary>
	public class ActivitiesJoinNotice : Base
	{
		private ActivitiesJoinNoticeData _data;
		public ActivitiesJoinNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class ActivitiesJoinNoticeData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;
		private DataItem _keyword4;

		/// <summary>
		/// 姓名
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 报名时间
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 活动时间
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
		/// <summary>
		/// 联系电话
		/// </summary>
		public DataItem keyword4 { get { return _keyword4; } set { _keyword4 = value; } }
	}
}
