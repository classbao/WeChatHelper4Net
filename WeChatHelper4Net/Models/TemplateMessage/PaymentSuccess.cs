using System;

/*
 * 微信公众账号API，发送模板消息模型
 * 作者：熊学浩
 * 时间：2014-6-25
 */
namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 订单支付成功通知
	/// </summary>
	public class PaymentSuccess : Base
	{
		private PaymentSuccessData _data;
		public PaymentSuccessData data { get { return _data; } set { _data = value; } }
	}

	public class PaymentSuccessData : DataBase
	{
		private DataItem _keyword1;
		private DataItem _keyword2;
		private DataItem _keyword3;
		private DataItem _keyword4;

		/// <summary>
		/// 订单商品
		/// </summary>
		public DataItem keyword1 { get { return _keyword1; } set { _keyword1 = value; } }
		/// <summary>
		/// 订单编号
		/// </summary>
		public DataItem keyword2 { get { return _keyword2; } set { _keyword2 = value; } }
		/// <summary>
		/// 支付金额
		/// </summary>
		public DataItem keyword3 { get { return _keyword3; } set { _keyword3 = value; } }
		/// <summary>
		/// 支付时间
		/// </summary>
		public DataItem keyword4 { get { return _keyword4; } set { _keyword4 = value; } }
	}
}
