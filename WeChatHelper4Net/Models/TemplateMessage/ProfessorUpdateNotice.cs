using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 教授名单更新通知
	/// </summary>
	public class ProfessorUpdateNotice : Base
	{
		private ProfessorUpdateNoticeData _data;
		public ProfessorUpdateNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class ProfessorUpdateNoticeData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;

		/// <summary>
		/// 教授姓名
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 所在单位
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 加入时间
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
	}
}
