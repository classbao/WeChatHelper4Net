using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 学术科研成果跟踪通知
	/// </summary>
	public class Track : Base
	{
		private TrackData _data;
		public TrackData data { get { return _data; } set { _data = value; } }
	}

	public class TrackData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;

		/// <summary>
		/// 科研方向
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 成果入库时间
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
	}
}
