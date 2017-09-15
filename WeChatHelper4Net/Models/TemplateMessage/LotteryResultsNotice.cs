using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2015-10-26
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 开奖结果通知
	/// </summary>
	public class LotteryResultsNotice : Base
	{
		private LotteryResultsNoticeData _data;
		public LotteryResultsNoticeData data { get { return _data; } set { _data = value; } }
	}

	public class LotteryResultsNoticeData : DataBase
	{
		private DataItem _program;
		private DataItem _result;

		/// <summary>
		/// 开奖项目
		/// </summary>
		public DataItem program { get { return _program; } set { _program = value; } }
		/// <summary>
		/// 中奖详情
		/// </summary>
		public DataItem result { get { return _result; } set { _result = value; } }
	}
}
