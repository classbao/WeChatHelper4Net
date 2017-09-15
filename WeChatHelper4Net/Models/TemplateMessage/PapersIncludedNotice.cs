using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 论文收录结果通知
	/// </summary>
	public class PapersIncludedNotice : Base
	{
		private PapersIncludedNoticeData _data;
		public PapersIncludedNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class PapersIncludedNoticeData : DataBase
	{
		private DataItem _paperName;
		private DataItem _name;
		private DataItem _result;

		/// <summary>
		/// 论文名称
		/// </summary>
		public DataItem paperName { get { return _paperName; } set { _paperName = value; } }
		/// <summary>
		/// 论文作者
		/// </summary>
		public DataItem name { get { return _name; } set { _name = value; } }
		/// <summary>
		/// 收录结果
		/// </summary>
		public DataItem result { get { return _result; } set { _result = value; } }
	}
}
