using System;

namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 用户位置信息模型
	/// </summary>
	public class UserLocationModel
	{
		private string _FromUserName = string.Empty;

		private decimal _Latitude;
		private decimal _Longitude;
		private decimal _Precision;
                
		private decimal _Location_X;
		private decimal _Location_Y;
		private int _Scale;
		private string _Label = string.Empty;

		private int _CreateTime;

		/// <summary>
		/// 发送方帐号（一个OpenID）
		/// </summary>
		public string FromUserName { get { return _FromUserName; } set { _FromUserName = value; } }

		/// <summary>
		/// 地理位置纬度
		/// </summary>
		public decimal Latitude { get { return _Latitude; } set { _Latitude = value; } }
		/// <summary>
		/// 地理位置经度
		/// </summary>
		public decimal Longitude { get { return _Longitude; } set { _Longitude = value; } }
		/// <summary>
		/// 地理位置精度
		/// </summary>
		public decimal Precision { get { return _Precision; } set { _Precision = value; } }

		/// <summary>
		/// 地理位置纬度
		/// </summary>
		public decimal Location_X { get { return _Location_X; } set { _Location_X = value; } }
		/// <summary>
		/// 地理位置经度
		/// </summary>
		public decimal Location_Y { get { return _Location_Y; } set { _Location_Y = value; } }
		/// <summary>
		/// 地图缩放大小
		/// </summary>
		public int Scale { get { return _Scale; } set { _Scale = value; } }
		/// <summary>
		/// 地理位置信息
		/// </summary>
		public string Label { get { return _Label; } set { _Label = value; } }

		/// <summary>
		/// 消息创建时间 （整型）
		/// </summary>
		public int CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }
	}
}