using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 待办工作提醒
	/// </summary>
	public class WorkRemind : Base
	{
		private WorkRemindData _data;
		public WorkRemindData data { get { return _data; } set { _data = value; } }
	}

	public class WorkRemindData : DataBase
	{
		private DataItem _work;

		/// <summary>
		/// 待办工作
		/// </summary>
		public DataItem work { get { return _work; } set { _work = value; } }
	}
}
