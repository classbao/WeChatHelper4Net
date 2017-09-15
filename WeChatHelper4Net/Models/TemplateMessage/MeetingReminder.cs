using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 会议提醒
	/// </summary>
	public class MeetingReminder : Base
	{
		private MeetingReminderData _data;
		public MeetingReminderData data { get { return _data; } set { _data = value; } }
	}

	public class MeetingReminderData : DataBase
	{
		private DataItem _Topic;
		private DataItem _Time;
		private DataItem _Address;

		/// <summary>
		/// 会议主题
		/// </summary>
		public DataItem Topic { get { return _Topic; } set { _Topic = value; } }
		/// <summary>
		/// 会议时间
		/// </summary>
		public DataItem Time { get { return _Time; } set { _Time = value; } }
		/// <summary>
		/// 会议地点
		/// </summary>
		public DataItem Address { get { return _Address; } set { _Address = value; } }
	}
}
