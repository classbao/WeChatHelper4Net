using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 创建二维码ticket请求参数模型
	/// </summary>
	public class TicketModel : RequestResultBaseModel
	{
		private string _ticket = string.Empty;
		private int _expire_seconds;

		/// <summary>
		/// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码
		/// </summary>
		public string ticket { get { return _ticket; } set { _ticket = value; } }
		/// <summary>
		/// 二维码的有效时间，以秒为单位。最大不超过604800（即7天）
		/// </summary>
		public int expire_seconds { get { return _expire_seconds; } set { _expire_seconds = value; } }
	}
}