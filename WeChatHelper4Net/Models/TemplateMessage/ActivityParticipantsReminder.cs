using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 新活动参与者提醒
	/// </summary>
	public class ActivityParticipantsReminder : Base
	{
		private ActivityParticipantsReminderData _data;
		public ActivityParticipantsReminderData data { get { return _data; } set { _data = value; } }
	}

	public class ActivityParticipantsReminderData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;

		/// <summary>
		/// 参与者
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 参与时间
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 参与内容
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
	}
}
